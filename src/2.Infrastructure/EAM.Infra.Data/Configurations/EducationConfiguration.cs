using EAM.Core.Domain.Entities.Resume;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAM.Infra.Data.Configurations;

public class EducationConfiguration : IEntityTypeConfiguration<Education>
{
    public void Configure(EntityTypeBuilder<Education> builder)
    {
        builder.ToTable("Educations");

        builder.Property(e => e.Degree)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(e => e.Institution)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Description)
            .HasMaxLength(1000);
    }
}
