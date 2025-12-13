namespace EAM.Core.Domain.Entities.Resume;

public class Skill : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public SkillCategory Category { get; set; }
    public int DisplayOrder { get; set; }
}

public enum SkillCategory
{
    PowerPlatform = 1,
    Development = 2,
    Other = 3
}
