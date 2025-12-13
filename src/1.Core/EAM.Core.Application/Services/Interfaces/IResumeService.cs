using EAM.Core.Application.DTOs.Resume;

namespace EAM.Core.Application.Services.Interfaces;

public interface IResumeService
{
    Task<ResumeDto> GetFullResumeAsync();
    Task<PersonalInfoDto?> GetPersonalInfoAsync();
    Task<List<ExperienceDto>> GetExperiencesAsync();
    Task<List<EducationDto>> GetEducationAsync();
    Task<CertificationsGroupDto> GetCertificationsAsync();
    Task<SkillsGroupDto> GetSkillsAsync();
    Task<List<LanguageDto>> GetLanguagesAsync();
}
