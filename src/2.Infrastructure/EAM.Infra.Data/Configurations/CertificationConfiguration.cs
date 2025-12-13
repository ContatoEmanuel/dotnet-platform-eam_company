using EAM.Core.Domain.Entities.Resume;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAM.Infra.Data.Configurations;

public class CertificationConfiguration : IEntityTypeConfiguration<Certification>
{
    public void Configure(EntityTypeBuilder<Certification> builder)
    {
        builder.ToTable("Certifications");

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(c => c.Issuer)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.CredentialCode)
            .HasMaxLength(100);

        builder.Property(c => c.CredentialUrl)
            .HasMaxLength(500);

        builder.Property(c => c.Description)
            .HasMaxLength(1000);

        builder.Property(c => c.Technologies)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
    }
}
