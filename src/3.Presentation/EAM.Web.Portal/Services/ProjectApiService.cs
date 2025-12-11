using System.Net.Http.Json;

namespace EAM.Web.Portal.Services;

public interface IProjectApiService
{
    Task<List<ProjectDto>> GetAllProjectsAsync();
    Task<List<ProjectDto>> GetFeaturedProjectsAsync();
    Task<List<ProjectDto>> GetOtherProjectsAsync();
    Task<ProjectDto?> GetProjectByIdAsync(int id);
    Task<ProjectDto?> CreateProjectAsync(ProjectCreateDto dto);
    Task<ProjectDto?> UpdateProjectAsync(int id, ProjectCreateDto dto);
    Task<bool> DeleteProjectAsync(int id);
}

public class ProjectApiService : IProjectApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProjectApiService> _logger;

    public ProjectApiService(HttpClient httpClient, ILogger<ProjectApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<ProjectDto>> GetAllProjectsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/projects");
            
            if (response.IsSuccessStatusCode)
            {
                var projects = await response.Content.ReadFromJsonAsync<List<ProjectDto>>();
                return projects ?? new List<ProjectDto>();
            }

            _logger.LogWarning("Falha ao obter projetos. Status: {StatusCode}", response.StatusCode);
            return new List<ProjectDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter projetos da API");
            return new List<ProjectDto>();
        }
    }

    public async Task<List<ProjectDto>> GetFeaturedProjectsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/projects/featured");
            
            if (response.IsSuccessStatusCode)
            {
                var projects = await response.Content.ReadFromJsonAsync<List<ProjectDto>>();
                return projects ?? new List<ProjectDto>();
            }

            _logger.LogWarning("Falha ao obter projetos em destaque. Status: {StatusCode}", response.StatusCode);
            return new List<ProjectDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter projetos em destaque da API");
            return new List<ProjectDto>();
        }
    }

    public async Task<List<ProjectDto>> GetOtherProjectsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/projects/other");
            
            if (response.IsSuccessStatusCode)
            {
                var projects = await response.Content.ReadFromJsonAsync<List<ProjectDto>>();
                return projects ?? new List<ProjectDto>();
            }

            _logger.LogWarning("Falha ao obter outros projetos. Status: {StatusCode}", response.StatusCode);
            return new List<ProjectDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter outros projetos da API");
            return new List<ProjectDto>();
        }
    }

    public async Task<ProjectDto?> GetProjectByIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/projects/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ProjectDto>();
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter projeto {ProjectId} da API", id);
            return null;
        }
    }

    public async Task<ProjectDto?> CreateProjectAsync(ProjectCreateDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/projects", dto);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ProjectDto>();
            }

            _logger.LogWarning("Falha ao criar projeto. Status: {StatusCode}", response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar projeto via API");
            return null;
        }
    }

    public async Task<ProjectDto?> UpdateProjectAsync(int id, ProjectCreateDto dto)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/projects/{id}", dto);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ProjectDto>();
            }

            _logger.LogWarning("Falha ao atualizar projeto {ProjectId}. Status: {StatusCode}", id, response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar projeto {ProjectId} via API", id);
            return null;
        }
    }

    public async Task<bool> DeleteProjectAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/projects/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao deletar projeto {ProjectId} via API", id);
            return false;
        }
    }
}

// DTOs
public class ProjectDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public string? ProjectUrl { get; set; }
    public string? RepositoryUrl { get; set; }
    public List<string> Technologies { get; set; } = new();
    public bool IsFeatured { get; set; }
    public bool ForSale { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class ProjectCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public string? ProjectUrl { get; set; }
    public string? RepositoryUrl { get; set; }
    public List<string> Technologies { get; set; } = new();
    public bool IsFeatured { get; set; }
    public bool ForSale { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
}
