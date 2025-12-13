namespace EAM.Core.Application.DTOs.Resume;

public class ResumeDto
{
    public PersonalInfoDto PersonalInfo { get; set; } = new();
    public List<ExperienceDto> Experiences { get; set; } = new();
    public List<EducationDto> Education { get; set; } = new();
    public CertificationsGroupDto Certifications { get; set; } = new();
    public SkillsGroupDto Skills { get; set; } = new();
    public List<LanguageDto> Languages { get; set; } = new();
}

public class CertificationsGroupDto
{
    public List<CertificationDto> MicrosoftCertified { get; set; } = new();
    public List<CertificationDto> MicrosoftAppliedSkills { get; set; } = new();
    public List<CertificationDto> Others { get; set; } = new();
}

public class SkillsGroupDto
{
    public List<SkillDto> PowerPlatform { get; set; } = new();
    public List<SkillDto> Development { get; set; } = new();
    public List<SkillDto> Others { get; set; } = new();
}
