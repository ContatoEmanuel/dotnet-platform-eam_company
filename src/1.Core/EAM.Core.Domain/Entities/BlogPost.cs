namespace EAM.Core.Domain.Entities;

public class BlogPost
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? TitleEn { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string? SlugEn { get; set; }
    public string Excerpt { get; set; } = string.Empty;
    public string? ExcerptEn { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? ContentEn { get; set; }
    public string? ImageUrl { get; set; }
    public string Author { get; set; } = "Emanuel Macêdo";
    public int ReadTimeMinutes { get; set; } = 5;
    public bool IsPublished { get; set; } = false;
    public DateTime? PublishedAt { get; set; }
    public int ViewCount { get; set; } = 0;
    public int DisplayOrder { get; set; } = 0;
    
    // Category
    public int CategoryId { get; set; }
    public BlogCategory? Category { get; set; }
    
    // Tags
    public string Tags { get; set; } = string.Empty; // JSON array serializado
    
    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
}

public class BlogCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? NameEn { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string? SlugEn { get; set; }
    public string? Description { get; set; }
    public string? DescriptionEn { get; set; }
    public string Color { get; set; } = "#3B82F6"; // Cor em hex
    public int DisplayOrder { get; set; } = 0;
    
    // Navigation
    public ICollection<BlogPost> Posts { get; set; } = new List<BlogPost>();
    
    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
}
