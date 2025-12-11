using System.Net.Http.Json;
using System.Text.Json;

namespace EAM.Web.Portal.Services;

public interface IAuthApiService
{
    Task<LoginResponse?> LoginAsync(string email, string password, bool rememberMe);
    Task<RegisterResponse?> RegisterAsync(string email, string password);
    Task<bool> LogoutAsync();
}

public class AuthApiService : IAuthApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AuthApiService> _logger;

    public AuthApiService(HttpClient httpClient, ILogger<AuthApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<LoginResponse?> LoginAsync(string email, string password, bool rememberMe)
    {
        try
        {
            var request = new
            {
                email = email,
                password = password
            };

            var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                return result;
            }

            _logger.LogWarning("Falha ao fazer login. Status: {StatusCode}", response.StatusCode);
            
            // Tentar ler mensagem de erro
            try
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                var errorObj = JsonSerializer.Deserialize<ErrorResponse>(errorContent);
                return new LoginResponse 
                { 
                    Success = false, 
                    Message = errorObj?.Message ?? "E-mail ou senha inválidos" 
                };
            }
            catch
            {
                return new LoginResponse { Success = false, Message = "E-mail ou senha inválidos" };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao fazer login via API");
            return new LoginResponse { Success = false, Message = "Erro ao conectar com o servidor" };
        }
    }

    public async Task<RegisterResponse?> RegisterAsync(string email, string password)
    {
        try
        {
            var request = new
            {
                email = email,
                password = password
            };

            var response = await _httpClient.PostAsJsonAsync("api/auth/register", request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<RegisterResponse>();
                return result;
            }

            _logger.LogWarning("Falha ao registrar. Status: {StatusCode}", response.StatusCode);
            
            // Tentar ler mensagem de erro
            try
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                var errorObj = JsonSerializer.Deserialize<ErrorResponse>(errorContent);
                return new RegisterResponse 
                { 
                    Success = false, 
                    Message = errorObj?.Message ?? "Erro ao criar conta" 
                };
            }
            catch
            {
                return new RegisterResponse { Success = false, Message = "Erro ao criar conta" };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar via API");
            return new RegisterResponse { Success = false, Message = "Erro ao conectar com o servidor" };
        }
    }

    public async Task<bool> LogoutAsync()
    {
        try
        {
            var response = await _httpClient.PostAsync("api/auth/logout", null);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao fazer logout via API");
            return false;
        }
    }
}

public class LoginResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
}

public class RegisterResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}

public class ErrorResponse
{
    public string Message { get; set; } = string.Empty;
}
