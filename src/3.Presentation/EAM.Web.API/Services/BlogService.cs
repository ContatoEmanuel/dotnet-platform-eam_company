using EAM.Core.Application.DTOs.Blog;
using EAM.Core.Application.Services.Interfaces;
using EAM.Core.Domain.Entities;
using EAM.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace EAM.Web.API.Services;

public class BlogService : IBlogService
{
    private readonly ApplicationDbContext _context;

    public BlogService(ApplicationDbContext context)
    {
        _context = context;
    }

    #region Posts

    public async Task<IEnumerable<BlogPostDto>> GetAllPostsAsync()
    {
        var posts = await _context.BlogPosts
            .Include(p => p.Category)
            .Where(p => p.IsActive)
            .OrderByDescending(p => p.PublishedAt ?? p.CreatedAt)
            .ToListAsync();

        return posts.Select(MapToDto);
    }

    public async Task<IEnumerable<BlogPostDto>> GetPublishedPostsAsync()
    {
        var posts = await _context.BlogPosts
            .Include(p => p.Category)
            .Where(p => p.IsActive && p.IsPublished && p.PublishedAt <= DateTime.UtcNow)
            .OrderByDescending(p => p.PublishedAt)
            .ToListAsync();

        return posts.Select(MapToDto);
    }

    public async Task<BlogPostDto?> GetPostByIdAsync(int id)
    {
        var post = await _context.BlogPosts
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

        return post != null ? MapToDto(post) : null;
    }

    public async Task<BlogPostDto?> GetPostBySlugAsync(string slug)
    {
        var post = await _context.BlogPosts
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Slug == slug && p.IsActive && p.IsPublished);

        return post != null ? MapToDto(post) : null;
    }

    public async Task<BlogPostDto> CreatePostAsync(BlogPostCreateDto postDto)
    {
        var post = new BlogPost
        {
            Title = postDto.Title,
            Slug = postDto.Slug,
            Excerpt = postDto.Excerpt,
            Content = postDto.Content,
            ImageUrl = postDto.ImageUrl,
            Author = postDto.Author,
            ReadTimeMinutes = postDto.ReadTimeMinutes,
            IsPublished = postDto.IsPublished,
            PublishedAt = postDto.PublishedAt,
            CategoryId = postDto.CategoryId,
            Tags = JsonSerializer.Serialize(postDto.Tags),
            DisplayOrder = postDto.DisplayOrder,
            CreatedAt = DateTime.UtcNow
        };

        _context.BlogPosts.Add(post);
        await _context.SaveChangesAsync();

        // Reload com Category
        await _context.Entry(post).Reference(p => p.Category).LoadAsync();

        return MapToDto(post);
    }

    public async Task<BlogPostDto?> UpdatePostAsync(int id, BlogPostUpdateDto postDto)
    {
        var post = await _context.BlogPosts.FindAsync(id);
        if (post == null) return null;

        if (postDto.Title != null) post.Title = postDto.Title;
        if (postDto.Excerpt != null) post.Excerpt = postDto.Excerpt;
        if (postDto.Content != null) post.Content = postDto.Content;
        if (postDto.ImageUrl != null) post.ImageUrl = postDto.ImageUrl;
        if (postDto.ReadTimeMinutes.HasValue) post.ReadTimeMinutes = postDto.ReadTimeMinutes.Value;
        if (postDto.IsPublished.HasValue) post.IsPublished = postDto.IsPublished.Value;
        if (postDto.PublishedAt.HasValue) post.PublishedAt = postDto.PublishedAt;
        if (postDto.CategoryId.HasValue) post.CategoryId = postDto.CategoryId.Value;
        if (postDto.Tags != null) post.Tags = JsonSerializer.Serialize(postDto.Tags);

        post.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        await _context.Entry(post).Reference(p => p.Category).LoadAsync();

        return MapToDto(post);
    }

    public async Task<bool> DeletePostAsync(int id)
    {
        var post = await _context.BlogPosts.FindAsync(id);
        if (post == null) return false;

        post.IsActive = false;
        post.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IncrementViewCountAsync(int id)
    {
        var post = await _context.BlogPosts.FindAsync(id);
        if (post == null) return false;

        post.ViewCount++;
        await _context.SaveChangesAsync();
        return true;
    }

    #endregion

    #region Categories

    public async Task<IEnumerable<BlogCategoryDto>> GetAllCategoriesAsync()
    {
        var categories = await _context.BlogCategories
            .Where(c => c.IsActive)
            .OrderBy(c => c.DisplayOrder)
            .ThenBy(c => c.Name)
            .ToListAsync();

        return categories.Select(MapCategoryToDto);
    }

    public async Task<BlogCategoryDto?> GetCategoryByIdAsync(int id)
    {
        var category = await _context.BlogCategories
            .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);

        return category != null ? MapCategoryToDto(category) : null;
    }

    public async Task<BlogCategoryDto?> GetCategoryBySlugAsync(string slug)
    {
        var category = await _context.BlogCategories
            .FirstOrDefaultAsync(c => c.Slug == slug && c.IsActive);

        return category != null ? MapCategoryToDto(category) : null;
    }

    public async Task<IEnumerable<BlogPostDto>> GetPostsByCategoryAsync(int categoryId)
    {
        var posts = await _context.BlogPosts
            .Include(p => p.Category)
            .Where(p => p.CategoryId == categoryId && p.IsActive && p.IsPublished)
            .OrderByDescending(p => p.PublishedAt)
            .ToListAsync();

        return posts.Select(MapToDto);
    }

    #endregion

    #region Mappers

    private static BlogPostDto MapToDto(BlogPost post)
    {
        return new BlogPostDto
        {
            Id = post.Id,
            Title = post.Title,
            Slug = post.Slug,
            Excerpt = post.Excerpt,
            Content = post.Content,
            ImageUrl = post.ImageUrl,
            Author = post.Author,
            ReadTimeMinutes = post.ReadTimeMinutes,
            PublishedAt = post.PublishedAt,
            ViewCount = post.ViewCount,
            Category = post.Category != null ? MapCategoryToDto(post.Category) : null,
            Tags = string.IsNullOrEmpty(post.Tags) 
                ? new List<string>() 
                : JsonSerializer.Deserialize<List<string>>(post.Tags) ?? new List<string>()
        };
    }

    private static BlogCategoryDto MapCategoryToDto(BlogCategory category)
    {
        return new BlogCategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Slug = category.Slug,
            Description = category.Description,
            Color = category.Color
        };
    }

    #endregion
}
