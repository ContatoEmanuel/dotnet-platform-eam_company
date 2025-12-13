using Microsoft.AspNetCore.Mvc;
using EAM.Core.Application.DTOs.Blog;
using System.Text.Json;

namespace EAM.Web.Public.Controllers;

public class BlogController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public BlogController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClient = httpClientFactory.CreateClient();
        _configuration = configuration;
        _httpClient.BaseAddress = new Uri(_configuration["ApiBaseUrl"] ?? "http://localhost:5000");
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

    public async Task<IActionResult> Post(int id)
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
                    ViewBag.Post = post;
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
