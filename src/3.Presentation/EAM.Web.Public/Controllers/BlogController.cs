using Microsoft.AspNetCore.Mvc;
using EAM.Core.Application.DTOs.Blog;
using System.Text.Json;
using Microsoft.Extensions.Options;
using EAM.Web.Public.Configuration;

namespace EAM.Web.Public.Controllers;

public class BlogController : BaseController
{
    private readonly HttpClient _httpClient;
    private readonly ApiSettings _apiSettings;

    public BlogController(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> apiSettings, IOptions<AppSettings> appSettings) : base(appSettings)
    {
        _httpClient = httpClientFactory.CreateClient();
        _apiSettings = apiSettings.Value;
        _httpClient.BaseAddress = new Uri(_apiSettings.BaseUrl);
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var response = await _httpClient.GetAsync("/api/blog/posts?publishedOnly=true");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var posts = JsonSerializer.Deserialize<List<BlogPostDto>>(json, new JsonSerializerOptions 
                { 
                    PropertyNameCaseInsensitive = true 
                });

                ViewBag.Posts = posts ?? new List<BlogPostDto>();
            }
            else
            {
                ViewBag.Posts = new List<BlogPostDto>();
                ViewBag.Error = "Não foi possível carregar os posts do blog.";
            }
        }
        catch (Exception ex)
        {
            ViewBag.Posts = new List<BlogPostDto>();
            ViewBag.Error = $"Erro ao conectar com a API: {ex.Message}";
        }

        return View();
    }

    [Route("Blog/Post/{id}/{slug?}")]
    public async Task<IActionResult> Post(int id, string? slug = null)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/blog/posts/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var post = JsonSerializer.Deserialize<BlogPostDto>(json, new JsonSerializerOptions 
                { 
                    PropertyNameCaseInsensitive = true 
                });

                if (post != null)
                {
                    // Redirect to URL with slug if slug is missing
                    if (string.IsNullOrEmpty(slug) && !string.IsNullOrEmpty(post.Slug))
                    {
                        return RedirectToActionPermanent("Post", new { id = id, slug = post.Slug });
                    }

                    ViewBag.Post = post;

                    // Get next post
                    try
                    {
                        var allPostsResponse = await _httpClient.GetAsync("/api/blog/posts?publishedOnly=true");
                        if (allPostsResponse.IsSuccessStatusCode)
                        {
                            var allPostsJson = await allPostsResponse.Content.ReadAsStringAsync();
                            var allPosts = JsonSerializer.Deserialize<List<BlogPostDto>>(allPostsJson, new JsonSerializerOptions 
                            { 
                                PropertyNameCaseInsensitive = true 
                            });

                            if (allPosts != null && allPosts.Count > 0)
                            {
                                var currentIndex = allPosts.FindIndex(p => p.Id == id);
                                if (currentIndex >= 0 && currentIndex < allPosts.Count - 1)
                                {
                                    ViewBag.NextPost = allPosts[currentIndex + 1];
                                }
                            }
                        }
                    }
                    catch
                    {
                        // Ignore error getting next post
                    }

                    return View();
                }
            }

            return NotFound();
        }
        catch (Exception ex)
        {
            ViewBag.Error = $"Erro ao carregar o post: {ex.Message}";
            return View("Error");
        }
    }

    public async Task<IActionResult> Category(string slug)
    {
        try
        {
            // Buscar categoria
            var categoryResponse = await _httpClient.GetAsync($"/api/blog/categories/slug/{slug}");
            if (!categoryResponse.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var categoryJson = await categoryResponse.Content.ReadAsStringAsync();
            var category = JsonSerializer.Deserialize<BlogCategoryDto>(categoryJson, new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true 
            });

            if (category == null) return NotFound();

            // Buscar posts da categoria
            var postsResponse = await _httpClient.GetAsync($"/api/blog/categories/{category.Id}/posts");
            var postsJson = await postsResponse.Content.ReadAsStringAsync();
            var posts = JsonSerializer.Deserialize<List<BlogPostDto>>(postsJson, new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true 
            });

            ViewBag.Category = category;
            ViewBag.Posts = posts ?? new List<BlogPostDto>();

            return View("Index");
        }
        catch (Exception ex)
        {
            ViewBag.Error = $"Erro ao carregar a categoria: {ex.Message}";
            return View("Error");
        }
    }
}
