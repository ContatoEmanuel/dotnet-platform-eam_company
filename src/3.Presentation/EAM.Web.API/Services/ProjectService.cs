using EAM.Core.Application.DTOs.Project;
using EAM.Core.Application.Services.Interfaces;
using EAM.Core.Domain.Entities;
using EAM.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace EAM.Web.API.Services;

public class ProjectService : IProjectService
{
    private readonly ApplicationDbContext _context;

    public ProjectService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync()
    {
        var projects = await _context.Projects
            .OrderBy(p => p.DisplayOrder)
            .ToListAsync();

        return projects.Select(MapToDto);
    }

    public async Task<IEnumerable<ProjectDto>> GetFeaturedProjectsAsync()
    {
        var projects = await _context.Projects
            .Where(p => p.IsFeatured)
            .OrderBy(p => p.DisplayOrder)
            .ToListAsync();

        return projects.Select(MapToDto);
    }

    public async Task<IEnumerable<ProjectDto>> GetOtherProjectsAsync()
    {
        var projects = await _context.Projects
            .Where(p => !p.IsFeatured)
            .OrderBy(p => p.DisplayOrder)
            .ToListAsync();

        return projects.Select(MapToDto);
    }

    public async Task<ProjectDto?> GetProjectByIdAsync(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        return project != null ? MapToDto(project) : null;
    }

    public async Task<ProjectDto> CreateProjectAsync(ProjectCreateDto projectDto)
    {
        var project = new Project
        {
            Title = projectDto.Title,
            Description = projectDto.Description,
            Technologies = JsonSerializer.Serialize(projectDto.Technologies),
            GithubUrl = projectDto.GithubUrl,
            LiveUrl = projectDto.LiveUrl,
            ThumbnailUrl = projectDto.ThumbnailUrl,
            IconEmoji = projectDto.IconEmoji,
            IsFeatured = projectDto.IsFeatured,
            DisplayOrder = projectDto.DisplayOrder,
            CreatedAt = DateTime.UtcNow
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        return MapToDto(project);
    }

    public async Task<ProjectDto?> UpdateProjectAsync(int id, ProjectCreateDto projectDto)
    {
        var project = await _context.Projects.FindAsync(id);
        
        if (project == null)
            return null;

        project.Title = projectDto.Title;
        project.Description = projectDto.Description;
        project.Technologies = JsonSerializer.Serialize(projectDto.Technologies);
        project.GithubUrl = projectDto.GithubUrl;
        project.LiveUrl = projectDto.LiveUrl;
        project.ThumbnailUrl = projectDto.ThumbnailUrl;
        project.IconEmoji = projectDto.IconEmoji;
        project.IsFeatured = projectDto.IsFeatured;
        project.DisplayOrder = projectDto.DisplayOrder;
        project.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToDto(project);
    }

    public async Task<bool> DeleteProjectAsync(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        
        if (project == null)
            return false;

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();

        return true;
    }

    private static ProjectDto MapToDto(Project project)
    {
        return new ProjectDto
        {
            Id = project.Id,
            Title = project.Title,
            Description = project.Description,
            Technologies = string.IsNullOrEmpty(project.Technologies) 
                ? new List<string>() 
                : JsonSerializer.Deserialize<List<string>>(project.Technologies) ?? new List<string>(),
            GithubUrl = project.GithubUrl,
            LiveUrl = project.LiveUrl,
            ThumbnailUrl = project.ThumbnailUrl,
            IconEmoji = project.IconEmoji,
            IsFeatured = project.IsFeatured,
            DisplayOrder = project.DisplayOrder,
            CreatedAt = project.CreatedAt
        };
    }
}
