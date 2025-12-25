namespace EAM.Core.Domain.Entities;

public class Project
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? TitleEn { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? DescriptionEn { get; set; }
    public string Technologies { get; set; } = string.Empty; // JSON array serializado
    public string? GithubUrl { get; set; }
    public string? LiveUrl { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string IconEmoji { get; set; } = "📦";
    public bool IsFeatured { get; set; }
    public bool ForSale { get; set; } = false;
    public decimal? Price { get; set; }
    public int DisplayOrder { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
}
