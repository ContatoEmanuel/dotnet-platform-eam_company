using EAM.Core.Application.DTOs.Blog;

namespace EAM.Core.Application.Services.Interfaces;

public interface IBlogService
{
    // Posts
    Task<IEnumerable<BlogPostDto>> GetAllPostsAsync();
    Task<IEnumerable<BlogPostDto>> GetPublishedPostsAsync();
    Task<BlogPostDto?> GetPostByIdAsync(int id);
    Task<BlogPostDto?> GetPostBySlugAsync(string slug);
    Task<BlogPostDto> CreatePostAsync(BlogPostCreateDto postDto);
    Task<BlogPostDto?> UpdatePostAsync(int id, BlogPostUpdateDto postDto);
    Task<bool> DeletePostAsync(int id);
    Task<bool> IncrementViewCountAsync(int id);
    
    // Categories
    Task<IEnumerable<BlogCategoryDto>> GetAllCategoriesAsync();
    Task<BlogCategoryDto?> GetCategoryByIdAsync(int id);
    Task<BlogCategoryDto?> GetCategoryBySlugAsync(string slug);
    Task<IEnumerable<BlogPostDto>> GetPostsByCategoryAsync(int categoryId);
}
