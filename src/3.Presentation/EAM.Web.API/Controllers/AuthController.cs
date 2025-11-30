using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EAM.Core.Application.DTOs.Auth;
using EAM.Core.Application.Services.Interfaces;
using System.Security.Claims;

namespace EAM.Web.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> _logger)
    {
        _authService = authService;
        this._logger = _logger;
    }

    /// <summary>
    /// Realiza login de usuário
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = await _authService.LoginAsync(request);
            
            if (!result.Success)
                return Unauthorized(new { message = result.Message });

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao fazer login");
            return StatusCode(500, new { message = "Erro interno no servidor" });
        }
    }

    /// <summary>
    /// Registra novo usuário (sempre como Client)
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = await _authService.RegisterAsync(request);
            
            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return CreatedAtAction(nameof(GetUserInfo), new { userId = result.UserInfo?.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar usuário");
            return StatusCode(500, new { message = "Erro interno no servidor" });
        }
    }

    /// <summary>
    /// Realiza logout do usuário atual
    /// </summary>
    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Logout()
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            await _authService.LogoutAsync(userId);
            
            return Ok(new { message = "Logout realizado com sucesso" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao fazer logout");
            return StatusCode(500, new { message = "Erro interno no servidor" });
        }
    }

    /// <summary>
    /// Obtém informações do usuário atual
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(UserInfoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCurrentUser()
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var userInfo = await _authService.GetUserInfoAsync(userId);
            
            if (userInfo == null)
                return NotFound(new { message = "Usuário não encontrado" });

            return Ok(userInfo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar informações do usuário");
            return StatusCode(500, new { message = "Erro interno no servidor" });
        }
    }

    /// <summary>
    /// Obtém informações de um usuário específico (somente Admin)
    /// </summary>
    [HttpGet("user/{userId}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(UserInfoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserInfo(string userId)
    {
        try
        {
            var userInfo = await _authService.GetUserInfoAsync(userId);
            
            if (userInfo == null)
                return NotFound(new { message = "Usuário não encontrado" });

            return Ok(userInfo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar informações do usuário {UserId}", userId);
            return StatusCode(500, new { message = "Erro interno no servidor" });
        }
    }

    /// <summary>
    /// Valida se um token JWT é válido
    /// </summary>
    [HttpPost("validate-token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ValidateToken([FromBody] string token)
    {
        try
        {
            var isValid = await _authService.ValidateTokenAsync(token);
            return Ok(new { valid = isValid });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao validar token");
            return BadRequest(new { valid = false, message = "Token inválido" });
        }
    }
}
