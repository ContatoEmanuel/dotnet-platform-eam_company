namespace EAM.Core.Application.DTOs.Blog;

public class BlogPostDto
{
    public int Id { get; set; }
    
    // Multiidioma - Português
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Excerpt { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    
    // Multiidioma - Inglês
    public string? TitleEn { get; set; }
    public string? SlugEn { get; set; }
    public string? ExcerptEn { get; set; }
    public string? ContentEn { get; set; }
    
    public string? ImageUrl { get; set; }
    public string Author { get; set; } = string.Empty;
    public int ReadTimeMinutes { get; set; }
    public DateTime? PublishedAt { get; set; }
    public int ViewCount { get; set; }
    public BlogCategoryDto? Category { get; set; }
    public List<string> Tags { get; set; } = new();
    
    /// <summary>
    /// Retorna o título no idioma solicitado
    /// </summary>
    public string GetTitle(string language = "pt-BR")
    {
        if (language == "en-US" && !string.IsNullOrEmpty(TitleEn))
            return TitleEn;
        return Title;
    }
    
    /// <summary>
    /// Retorna o excerpt no idioma solicitado
    /// </summary>
    public string GetExcerpt(string language = "pt-BR")
    {
        if (language == "en-US" && !string.IsNullOrEmpty(ExcerptEn))
            return ExcerptEn;
        return Excerpt;
    }
    
    /// <summary>
    /// Retorna o conteúdo no idioma solicitado
    /// </summary>
    public string GetContent(string language = "pt-BR")
    {
        if (language == "en-US" && !string.IsNullOrEmpty(ContentEn))
            return ContentEn;
        return Content;
    }
}

public class BlogCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? NameEn { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string? SlugEn { get; set; }
    public string? Description { get; set; }
    public string? DescriptionEn { get; set; }
    public string Color { get; set; } = "#3B82F6";
    
    /// <summary>
    /// Retorna o nome da categoria no idioma solicitado
    /// </summary>
    public string GetName(string language = "pt-BR")
    {
        if (language == "en-US" && !string.IsNullOrEmpty(NameEn))
            return NameEn;
        return Name;
    }
}

public class BlogPostCreateDto
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Excerpt { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string Author { get; set; } = string.Empty;
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
