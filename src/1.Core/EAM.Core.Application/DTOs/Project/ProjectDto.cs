namespace EAM.Core.Application.DTOs.Project;

public class ProjectDto
{
    public int Id { get; set; }
    
    // Multiidioma - Português
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    // Multiidioma - Inglês
    public string? TitleEn { get; set; }
    public string? DescriptionEn { get; set; }
    
    public List<string> Technologies { get; set; } = new();
    public string? GithubUrl { get; set; }
    public string? LiveUrl { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string IconEmoji { get; set; } = "📦";
    public bool IsFeatured { get; set; }
    public bool ForSale { get; set; }
    public decimal? Price { get; set; }
    public bool IsActive { get; set; }
    public int DisplayOrder { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
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
    /// Retorna a descrição no idioma solicitado
    /// </summary>
    public string GetDescription(string language = "pt-BR")
    {
        if (language == "en-US" && !string.IsNullOrEmpty(DescriptionEn))
            return DescriptionEn;
        return Description;
    }
}
