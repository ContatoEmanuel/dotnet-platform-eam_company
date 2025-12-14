namespace EAM.Core.Application.DTOs.Resume;

public class CertificationDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public string Period { get; set; } = string.Empty;
    public string? CredentialCode { get; set; }
    public string? CredentialUrl { get; set; }
    public string Description { get; set; } = string.Empty;
    public List<string> Technologies { get; set; } = new();
    public string Type { get; set; } = string.Empty;
}
