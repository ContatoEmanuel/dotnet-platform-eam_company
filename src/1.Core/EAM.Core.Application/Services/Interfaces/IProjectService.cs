using EAM.Core.Application.DTOs.Project;

namespace EAM.Core.Application.Services.Interfaces;

public interface IProjectService
{
    Task<IEnumerable<ProjectDto>> GetAllProjectsAsync();
    Task<IEnumerable<ProjectDto>> GetFeaturedProjectsAsync();
    Task<IEnumerable<ProjectDto>> GetOtherProjectsAsync();
    Task<ProjectDto?> GetProjectByIdAsync(int id);
    Task<ProjectDto> CreateProjectAsync(ProjectCreateDto projectDto);
    Task<ProjectDto?> UpdateProjectAsync(int id, ProjectCreateDto projectDto);
    Task<bool> DeleteProjectAsync(int id);
}
