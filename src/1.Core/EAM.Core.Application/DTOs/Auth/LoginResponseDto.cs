namespace EAM.Core.Application.DTOs.Auth;

public class LoginResponseDto
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? Expiration { get; set; }
    public UserInfoDto? UserInfo { get; set; }
    public string? Message { get; set; }
}

public class UserInfoDto
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public List<string> Roles { get; set; } = new();
}
