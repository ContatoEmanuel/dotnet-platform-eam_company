using EAM.Core.Domain.Entities.Resume;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAM.Infra.Data.Configurations;

public class ExperienceConfiguration : IEntityTypeConfiguration<Experience>
{
    public void Configure(EntityTypeBuilder<Experience> builder)
    {
        builder.ToTable("Experiences");

        builder.Property(e => e.JobTitle)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Company)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Location)
            .HasMaxLength(200);

        builder.Property(e => e.Description)
            .HasMaxLength(2000);

        builder.Property(e => e.Technologies)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
    }
}
