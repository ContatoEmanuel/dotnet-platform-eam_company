namespace EAM.Core.Domain.Entities;

public class Lead
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Company { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Subject { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsContacted { get; set; } = false;
    public DateTime? ContactedAt { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; } = true;
}
