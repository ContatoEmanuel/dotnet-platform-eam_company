using EAM.Core.Application.DTOs.Auth;

namespace EAM.Core.Application.Services.Interfaces;

public interface IAuthService
{
    /// <summary>
    /// Realiza login de usuário e retorna JWT token
    /// </summary>
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);

    /// <summary>
    /// Registra novo usuário como Cliente (não Admin)
    /// </summary>
    Task<LoginResponseDto> RegisterAsync(RegisterRequestDto request);

    /// <summary>
    /// Realiza logout do usuário (invalida token se necessário)
    /// </summary>
    Task<bool> LogoutAsync(string userId);

    /// <summary>
    /// Valida se um token JWT é válido
    /// </summary>
    Task<bool> ValidateTokenAsync(string token);

    /// <summary>
    /// Obtém informações do usuário atual pelo ID
    /// </summary>
    Task<UserInfoDto?> GetUserInfoAsync(string userId);
}
