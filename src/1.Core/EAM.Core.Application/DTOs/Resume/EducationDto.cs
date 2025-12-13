namespace EAM.Core.Application.DTOs.Resume;

public class EducationDto
{
    public int Id { get; set; }
    public string Degree { get; set; } = string.Empty;
    public string Institution { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsInProgress { get; set; }
    public string Period { get; set; } = string.Empty;
    public string? Description { get; set; }
}
