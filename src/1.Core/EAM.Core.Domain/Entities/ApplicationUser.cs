using Microsoft.AspNetCore.Identity;

namespace EAM.Core.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
    public string? CPF { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Audit fields
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
}
