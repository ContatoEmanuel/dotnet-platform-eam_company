namespace EAM.Core.Application.DTOs.Blog;

public class BlogPostDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Excerpt { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string Author { get; set; } = string.Empty;
    public int ReadTimeMinutes { get; set; }
    public DateTime? PublishedAt { get; set; }
    public int ViewCount { get; set; }
    public BlogCategoryDto? Category { get; set; }
    public List<string> Tags { get; set; } = new();
}

public class BlogCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Color { get; set; } = "#3B82F6";
}

public class BlogPostCreateDto
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Excerpt { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string Author { get; set; } = "Emanuel Macêdo";
    public int ReadTimeMinutes { get; set; } = 5;
    public bool IsPublished { get; set; } = false;
    public DateTime? PublishedAt { get; set; }
    public int CategoryId { get; set; }
    public List<string> Tags { get; set; } = new();
    public int DisplayOrder { get; set; } = 0;
}

public class BlogPostUpdateDto
{
    public string? Title { get; set; }
    public string? Excerpt { get; set; }
    public string? Content { get; set; }
    public string? ImageUrl { get; set; }
    public int? ReadTimeMinutes { get; set; }
    public bool? IsPublished { get; set; }
    public DateTime? PublishedAt { get; set; }
    public int? CategoryId { get; set; }
    public List<string>? Tags { get; set; }
}
