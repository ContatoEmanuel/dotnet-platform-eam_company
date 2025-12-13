using EAM.Core.Application.DTOs.Resume;
using EAM.Core.Application.Services.Interfaces;
using EAM.Core.Domain.Entities.Resume;
using EAM.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace EAM.Web.API.Services;

public class ResumeService : IResumeService
{
    private readonly ApplicationDbContext _context;

    public ResumeService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ResumeDto> GetFullResumeAsync()
    {
        return new ResumeDto
        {
            PersonalInfo = await GetPersonalInfoAsync() ?? new PersonalInfoDto(),
            Experiences = await GetExperiencesAsync(),
            Education = await GetEducationAsync(),
            Certifications = await GetCertificationsAsync(),
            Skills = await GetSkillsAsync(),
            Languages = await GetLanguagesAsync()
        };
    }

    public async Task<PersonalInfoDto?> GetPersonalInfoAsync()
    {
        var info = await _context.PersonalInfos
            .Where(p => p.IsActive)
            .FirstOrDefaultAsync();

        if (info == null) return null;

        return new PersonalInfoDto
        {
            FullName = info.FullName,
            Title = info.Title,
            Location = info.Location,
            Email = info.Email,
            Phone = info.Phone,
            LinkedIn = info.LinkedIn,
            GitHub = info.GitHub,
            Summary = info.Summary
        };
    }

    public async Task<List<ExperienceDto>> GetExperiencesAsync()
    {
        var experiences = await _context.Experiences
            .OrderBy(e => e.DisplayOrder)
            .ToListAsync();

        return experiences.Select(e => new ExperienceDto
        {
            Id = e.Id,
            JobTitle = e.JobTitle,
            Company = e.Company,
            Location = e.Location,
            StartDate = e.StartDate,
            EndDate = e.EndDate,
            IsCurrentJob = e.IsCurrentJob,
            Period = FormatPeriod(e.StartDate, e.EndDate, e.IsCurrentJob),
            Description = e.Description,
            Technologies = e.Technologies
        }).ToList();
    }

    public async Task<List<EducationDto>> GetEducationAsync()
    {
        var education = await _context.Educations
            .OrderBy(e => e.DisplayOrder)
            .ToListAsync();

        return education.Select(e => new EducationDto
        {
            Id = e.Id,
            Degree = e.Degree,
            Institution = e.Institution,
            StartDate = e.StartDate,
            EndDate = e.EndDate,
            IsInProgress = e.IsInProgress,
            Period = FormatPeriod(e.StartDate, e.EndDate, e.IsInProgress),
            Description = e.Description
        }).ToList();
    }

    public async Task<CertificationsGroupDto> GetCertificationsAsync()
    {
        var certifications = await _context.Certifications
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync();

        var certificationsDto = certifications.Select(c => new CertificationDto
        {
            Id = c.Id,
            Name = c.Name,
            Issuer = c.Issuer,
            IssueDate = c.IssueDate,
            Period = c.IssueDate.ToString("MMM/yyyy", new CultureInfo("pt-BR")).ToLower(),
            CredentialCode = c.CredentialCode,
            CredentialUrl = c.CredentialUrl,
            Description = c.Description,
            Technologies = c.Technologies,
            Type = c.Type.ToString()
        }).ToList();

        return new CertificationsGroupDto
        {
            MicrosoftCertified = certificationsDto
                .Where(c => c.Type == nameof(CertificationType.MicrosoftCertified))
                .ToList(),
            MicrosoftAppliedSkills = certificationsDto
                .Where(c => c.Type == nameof(CertificationType.MicrosoftAppliedSkills))
                .ToList(),
            Others = certificationsDto
                .Where(c => c.Type == nameof(CertificationType.Other))
                .ToList()
        };
    }

    public async Task<SkillsGroupDto> GetSkillsAsync()
    {
        var skills = await _context.Skills
            .OrderBy(s => s.DisplayOrder)
            .ToListAsync();

        var skillsDto = skills.Select(s => new SkillDto
        {
            Id = s.Id,
            Name = s.Name,
            Category = s.Category.ToString()
        }).ToList();

        return new SkillsGroupDto
        {
            PowerPlatform = skillsDto
                .Where(s => s.Category == nameof(SkillCategory.PowerPlatform))
                .ToList(),
            Development = skillsDto
                .Where(s => s.Category == nameof(SkillCategory.Development))
                .ToList(),
            Others = skillsDto
                .Where(s => s.Category == nameof(SkillCategory.Other))
                .ToList()
        };
    }

    public async Task<List<LanguageDto>> GetLanguagesAsync()
    {
        var languages = await _context.Languages
            .OrderBy(l => l.DisplayOrder)
            .ToListAsync();

        return languages.Select(l => new LanguageDto
        {
            Id = l.Id,
            Name = l.Name,
            ProficiencyLevel = l.ProficiencyLevel
        }).ToList();
    }

    private static string FormatPeriod(DateTime startDate, DateTime? endDate, bool isCurrent)
    {
        var culture = new CultureInfo("pt-BR");
        var start = startDate.ToString("MMM/yyyy", culture).ToLower();
        
        if (isCurrent)
            return $"{start} - presente";
        
        if (endDate.HasValue)
        {
            var end = endDate.Value.ToString("MMM/yyyy", culture).ToLower();
            return $"{start} - {end}";
        }

        return start;
    }
}
