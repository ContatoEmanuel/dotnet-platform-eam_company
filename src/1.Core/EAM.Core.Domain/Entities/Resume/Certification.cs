namespace EAM.Core.Domain.Entities.Resume;

public class Certification : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public string? CredentialCode { get; set; }
    public string? CredentialUrl { get; set; }
    public string Description { get; set; } = string.Empty;
    public List<string> Technologies { get; set; } = new();
    public CertificationType Type { get; set; }
    public int DisplayOrder { get; set; }
}

public enum CertificationType
{
    MicrosoftCertified = 1,
    MicrosoftAppliedSkills = 2,
    Other = 3
}
