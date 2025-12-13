namespace EAM.Core.Domain.Entities.Resume;

public class Language : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string ProficiencyLevel { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
}
