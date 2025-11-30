using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using EAM.Core.Application.DTOs.Auth;
using EAM.Core.Application.Services.Interfaces;
using EAM.Core.Domain.Entities;

namespace EAM.Web.API.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        
        if (user == null || !user.IsActive)
        {
            return new LoginResponseDto
            {
                Success = false,
                Message = "Email ou senha inválidos"
            };
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);

        if (!result.Succeeded)
        {
            return new LoginResponseDto
            {
                Success = false,
                Message = result.IsLockedOut 
                    ? "Conta bloqueada temporariamente. Tente novamente mais tarde."
                    : "Email ou senha inválidos"
            };
        }

        var token = await GenerateJwtTokenAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        return new LoginResponseDto
        {
            Success = true,
            Token = token,
            Expiration = DateTime.UtcNow.AddHours(
                _configuration.GetValue<int>("Jwt:ExpirationInHours", 24)),
            UserInfo = new UserInfoDto
            {
                Id = user.Id,
                Email = user.Email!,
                FullName = user.FullName,
                Roles = roles.ToList()
            },
            Message = "Login realizado com sucesso"
        };
    }

    public async Task<LoginResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return new LoginResponseDto
            {
                Success = false,
                Message = "Este email já está cadastrado"
            };
        }

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FullName = request.FullName,
            CPF = request.CPF,
            EmailConfirmed = false, // Confirmação por email pode ser implementada depois
            IsActive = true
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return new LoginResponseDto
            {
                Success = false,
                Message = string.Join(", ", result.Errors.Select(e => e.Description))
            };
        }

        // Novos usuários sempre começam como "Client"
        await _userManager.AddToRoleAsync(user, "Client");

        // Fazer login automaticamente após registro
        var token = await GenerateJwtTokenAsync(user);

        return new LoginResponseDto
        {
            Success = true,
            Token = token,
            Expiration = DateTime.UtcNow.AddHours(
                _configuration.GetValue<int>("Jwt:ExpirationInHours", 24)),
            UserInfo = new UserInfoDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Roles = new List<string> { "Client" }
            },
            Message = "Cadastro realizado com sucesso"
        };
    }

    public async Task<bool> LogoutAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        // Com JWT, logout é feito no client (remover token)
        // Aqui podemos implementar blacklist de tokens se necessário
        await _signInManager.SignOutAsync();
        
        return true;
    }

    public Task<bool> ValidateTokenAsync(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out _);

            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    public async Task<UserInfoDto?> GetUserInfoAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null || !user.IsActive)
            return null;

        var roles = await _userManager.GetRolesAsync(user);

        return new UserInfoDto
        {
            Id = user.Id,
            Email = user.Email!,
            FullName = user.FullName,
            Roles = roles.ToList()
        };
    }

    private async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.Name, user.FullName ?? user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Adicionar roles como claims
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddHours(
            _configuration.GetValue<int>("Jwt:ExpirationInHours", 24));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
