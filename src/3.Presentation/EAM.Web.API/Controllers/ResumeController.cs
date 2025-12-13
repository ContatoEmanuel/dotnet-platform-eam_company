using EAM.Core.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EAM.Web.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResumeController : ControllerBase
{
    private readonly IResumeService _resumeService;

    public ResumeController(IResumeService resumeService)
    {
        _resumeService = resumeService;
    }

    [HttpGet("full")]
    public async Task<IActionResult> GetFullResume()
    {
        var resume = await _resumeService.GetFullResumeAsync();
        return Ok(resume);
    }

    [HttpGet("personal-info")]
    public async Task<IActionResult> GetPersonalInfo()
    {
        var info = await _resumeService.GetPersonalInfoAsync();
        if (info == null)
            return NotFound("Personal info not found");
        
        return Ok(info);
    }

    [HttpGet("experiences")]
    public async Task<IActionResult> GetExperiences()
    {
        var experiences = await _resumeService.GetExperiencesAsync();
        return Ok(experiences);
    }

    [HttpGet("education")]
    public async Task<IActionResult> GetEducation()
    {
        var education = await _resumeService.GetEducationAsync();
        return Ok(education);
    }

    [HttpGet("certifications")]
    public async Task<IActionResult> GetCertifications()
    {
        var certifications = await _resumeService.GetCertificationsAsync();
        return Ok(certifications);
    }

    [HttpGet("skills")]
    public async Task<IActionResult> GetSkills()
    {
        var skills = await _resumeService.GetSkillsAsync();
        return Ok(skills);
    }

    [HttpGet("languages")]
    public async Task<IActionResult> GetLanguages()
    {
        var languages = await _resumeService.GetLanguagesAsync();
        return Ok(languages);
    }
}
