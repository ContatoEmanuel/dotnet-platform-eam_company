using EAM.Core.Application.DTOs.Blog;
using EAM.Core.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EAM.Web.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BlogController : ControllerBase
{
    private readonly IBlogService _blogService;

    public BlogController(IBlogService blogService)
    {
        _blogService = blogService;
    }

    // GET: api/blog/posts
    [HttpGet("posts")]
    public async Task<ActionResult<IEnumerable<BlogPostDto>>> GetPosts([FromQuery] bool publishedOnly = true, [FromQuery] string? language = "pt-BR")
    {
        var posts = publishedOnly 
            ? await _blogService.GetPublishedPostsAsync()
            : await _blogService.GetAllPostsAsync();
        
        // Normaliza a linguagem
        if (string.IsNullOrEmpty(language) || (language != "pt-BR" && language != "en-US"))
            language = "pt-BR";
            
        return Ok(posts);
    }

    // GET: api/blog/posts/5
    [HttpGet("posts/{id:int}")]
    public async Task<ActionResult<BlogPostDto>> GetPost(int id, [FromQuery] string? language = "pt-BR")
    {
        var post = await _blogService.GetPostByIdAsync(id);
        
        if (post == null)
            return NotFound(new { message = "Post não encontrado" });

        // Incrementar visualizações
        await _blogService.IncrementViewCountAsync(id);

        return Ok(post);
    }

    // GET: api/blog/posts/slug/meu-post
    [HttpGet("posts/slug/{slug}")]
    public async Task<ActionResult<BlogPostDto>> GetPostBySlug(string slug, [FromQuery] string? language = "pt-BR")
    {
        var post = await _blogService.GetPostBySlugAsync(slug);
        
        if (post == null)
            return NotFound(new { message = "Post não encontrado" });

        // Incrementar visualizações
        await _blogService.IncrementViewCountAsync(post.Id);

        return Ok(post);
    }

    // POST: api/blog/posts
    [HttpPost("posts")]
    public async Task<ActionResult<BlogPostDto>> CreatePost([FromBody] BlogPostCreateDto postDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdPost = await _blogService.CreatePostAsync(postDto);
        
        return CreatedAtAction(
            nameof(GetPost), 
            new { id = createdPost.Id }, 
            createdPost);
    }

    // PUT: api/blog/posts/5
    [HttpPut("posts/{id}")]
    public async Task<ActionResult<BlogPostDto>> UpdatePost(int id, [FromBody] BlogPostUpdateDto postDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updatedPost = await _blogService.UpdatePostAsync(id, postDto);
        
        if (updatedPost == null)
            return NotFound(new { message = "Post não encontrado" });

        return Ok(updatedPost);
    }

    // DELETE: api/blog/posts/5
    [HttpDelete("posts/{id}")]
    public async Task<ActionResult> DeletePost(int id)
    {
        var success = await _blogService.DeletePostAsync(id);
        
        if (!success)
            return NotFound(new { message = "Post não encontrado" });

        return NoContent();
    }

    // GET: api/blog/categories
    [HttpGet("categories")]
    public async Task<ActionResult<IEnumerable<BlogCategoryDto>>> GetCategories()
    {
        var categories = await _blogService.GetAllCategoriesAsync();
        return Ok(categories);
    }

    // GET: api/blog/categories/5
    [HttpGet("categories/{id:int}")]
    public async Task<ActionResult<BlogCategoryDto>> GetCategory(int id)
    {
        var category = await _blogService.GetCategoryByIdAsync(id);
        
        if (category == null)
            return NotFound(new { message = "Categoria não encontrada" });

        return Ok(category);
    }

    // GET: api/blog/categories/slug/dynamics-365
    [HttpGet("categories/slug/{slug}")]
    public async Task<ActionResult<BlogCategoryDto>> GetCategoryBySlug(string slug)
    {
        var category = await _blogService.GetCategoryBySlugAsync(slug);
        
        if (category == null)
            return NotFound(new { message = "Categoria não encontrada" });

        return Ok(category);
    }

    // GET: api/blog/categories/5/posts
    [HttpGet("categories/{id}/posts")]
    public async Task<ActionResult<IEnumerable<BlogPostDto>>> GetPostsByCategory(int id)
    {
        var posts = await _blogService.GetPostsByCategoryAsync(id);
        return Ok(posts);
    }
}
