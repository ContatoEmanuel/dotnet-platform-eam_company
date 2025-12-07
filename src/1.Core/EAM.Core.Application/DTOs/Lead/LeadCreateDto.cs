using System.ComponentModel.DataAnnotations;

namespace EAM.Core.Application.DTOs.Lead;

public class LeadCreateDto
{
    [Required(ErrorMessage = "Nome completo é obrigatório")]
    [StringLength(200, ErrorMessage = "Nome deve ter no máximo 200 caracteres")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    [StringLength(200, ErrorMessage = "Email deve ter no máximo 200 caracteres")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Telefone inválido")]
    [StringLength(20, ErrorMessage = "Telefone deve ter no máximo 20 caracteres")]
    public string? Phone { get; set; }

    [StringLength(200, ErrorMessage = "Empresa deve ter no máximo 200 caracteres")]
    public string? Company { get; set; }

    [Required(ErrorMessage = "Mensagem é obrigatória")]
    [StringLength(2000, MinimumLength = 10, ErrorMessage = "Mensagem deve ter entre 10 e 2000 caracteres")]
    public string Message { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "Assunto deve ter no máximo 200 caracteres")]
    public string? Subject { get; set; }
}
