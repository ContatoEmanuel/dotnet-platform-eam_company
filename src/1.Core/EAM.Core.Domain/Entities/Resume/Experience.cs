namespace EAM.Core.Domain.Entities.Resume;

public class Experience : BaseEntity
{
    public string JobTitle { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsCurrentJob { get; set; } = false;
    public string Description { get; set; } = string.Empty;
    public List<string> Technologies { get; set; } = new();
    public int DisplayOrder { get; set; }
}
