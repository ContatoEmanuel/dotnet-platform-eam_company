using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace EAM.Web.Portal.Services;

public interface IUserApiService
{
    Task<List<UserDto>> GetAllUsersAsync();
    Task<UserDto?> GetUserByIdAsync(string id);
    Task<bool> AddAdminRoleAsync(string userId);
    Task<bool> RemoveAdminRoleAsync(string userId);
}

public class UserApiService : IUserApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<UserApiService> _logger;

    public UserApiService(HttpClient httpClient, ILogger<UserApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/users");
            
            if (response.IsSuccessStatusCode)
            {
                var users = await response.Content.ReadFromJsonAsync<List<UserDto>>();
                return users ?? new List<UserDto>();
            }

            _logger.LogWarning("Falha ao obter usuários. Status: {StatusCode}", response.StatusCode);
            return new List<UserDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter usuários da API");
            return new List<UserDto>();
        }
    }

    public async Task<UserDto?> GetUserByIdAsync(string id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/users/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserDto>();
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter usuário {UserId} da API", id);
            return null;
        }
    }

    public async Task<bool> AddAdminRoleAsync(string userId)
    {
        try
        {
            var response = await _httpClient.PostAsync($"api/users/{userId}/roles/admin", null);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao adicionar role admin ao usuário {UserId}", userId);
            return false;
        }
    }

    public async Task<bool> RemoveAdminRoleAsync(string userId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/users/{userId}/roles/admin");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover role admin do usuário {UserId}", userId);
            return false;
        }
    }
}

public class UserDto
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool EmailConfirmed { get; set; }
    public List<string> Roles { get; set; } = new();
}
