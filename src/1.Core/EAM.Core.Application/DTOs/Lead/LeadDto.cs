namespace EAM.Core.Application.DTOs.Lead;

public class LeadDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Company { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Subject { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsContacted { get; set; }
    public DateTime? ContactedAt { get; set; }
}
