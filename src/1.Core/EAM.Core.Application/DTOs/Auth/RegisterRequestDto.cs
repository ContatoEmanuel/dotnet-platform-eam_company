using System.ComponentModel.DataAnnotations;

namespace EAM.Core.Application.DTOs.Auth;

public class RegisterRequestDto
{
    [Required(ErrorMessage = "Nome completo é obrigatório")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Nome deve ter entre 3 e 100 caracteres")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Senha é obrigatória")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Senha deve ter no mínimo 8 caracteres")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirmação de senha é obrigatória")]
    [Compare("Password", ErrorMessage = "Senha e confirmação não conferem")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = string.Empty;

    [StringLength(11, MinimumLength = 11, ErrorMessage = "CPF deve ter 11 dígitos")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "CPF deve conter apenas números")]
    public string? CPF { get; set; }
}
