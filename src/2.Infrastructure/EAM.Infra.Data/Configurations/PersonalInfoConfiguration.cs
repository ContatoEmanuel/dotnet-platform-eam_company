using EAM.Core.Domain.Entities.Resume;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAM.Infra.Data.Configurations;

public class PersonalInfoConfiguration : IEntityTypeConfiguration<PersonalInfo>
{
    public void Configure(EntityTypeBuilder<PersonalInfo> builder)
    {
        builder.ToTable("PersonalInfos");

        builder.Property(p => p.FullName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Location)
            .HasMaxLength(200);

        builder.Property(p => p.Email)
            .HasMaxLength(200);

        builder.Property(p => p.Phone)
            .HasMaxLength(50);

        builder.Property(p => p.LinkedIn)
            .HasMaxLength(500);

        builder.Property(p => p.GitHub)
            .HasMaxLength(500);

        builder.Property(p => p.Summary)
            .HasMaxLength(2000);
    }
}
