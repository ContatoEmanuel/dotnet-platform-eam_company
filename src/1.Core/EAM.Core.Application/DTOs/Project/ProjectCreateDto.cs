namespace EAM.Core.Application.DTOs.Project;

public class ProjectCreateDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Technologies { get; set; } = new();
    public string? GithubUrl { get; set; }
    public string? LiveUrl { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string IconEmoji { get; set; } = "📦";
    public bool IsFeatured { get; set; }
    public int DisplayOrder { get; set; }
}
