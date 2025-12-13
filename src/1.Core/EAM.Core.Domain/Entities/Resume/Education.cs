namespace EAM.Core.Domain.Entities.Resume;

public class Education : BaseEntity
{
    public string Degree { get; set; } = string.Empty;
    public string Institution { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsInProgress { get; set; } = false;
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }
}
