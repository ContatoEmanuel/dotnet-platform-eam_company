using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EAM.Core.Domain.Entities;
using System.Linq.Expressions;

namespace EAM.Infra.Data.Context;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSets serão adicionados conforme as entidades forem criadas
    // public DbSet<Project> Projects => Set<Project>();
    // public DbSet<Skill> Skills => Set<Skill>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // IMPORTANTE: chamada base para Identity
        
        // Aplicar configurações de entidades (Fluent API)
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        
        // Seed de Roles e usuário Admin
        SeedIdentityData(modelBuilder);
        
        // Configurações globais
        ConfigureGlobalFilters(modelBuilder);
    }

    private void SeedIdentityData(ModelBuilder modelBuilder)
    {
        // IDs fixos para consistência
        var adminRoleId = "a1b2c3d4-e5f6-4a5b-8c9d-0e1f2a3b4c5d";
        var clientRoleId = "b2c3d4e5-f6a7-5b6c-9d0e-1f2a3b4c5d6e";
        var adminUserId = "c3d4e5f6-a7b8-6c7d-0e1f-2a3b4c5d6e7f";

        // Seed Roles
        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Id = adminRoleId,
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = adminRoleId
            },
            new IdentityRole
            {
                Id = clientRoleId,
                Name = "Client",
                NormalizedName = "CLIENT",
                ConcurrencyStamp = clientRoleId
            }
        );

        // Seed Admin User
        // Hash pré-calculado para senha: Admin@123
        modelBuilder.Entity<ApplicationUser>().HasData(
            new ApplicationUser
            {
                Id = adminUserId,
                UserName = "admin@eam-company.com.br",
                NormalizedUserName = "ADMIN@EAM-COMPANY.COM.BR",
                Email = "admin@eam-company.com.br",
                NormalizedEmail = "ADMIN@EAM-COMPANY.COM.BR",
                EmailConfirmed = true,
                FullName = "Administrador EAM",
                CPF = "00000000000",
                IsActive = true,
                CreatedAt = new DateTime(2025, 11, 30, 0, 0, 0, DateTimeKind.Utc),
                SecurityStamp = "d4e5f6a7-b8c9-7d8e-1f2a-3b4c5d6e7f8a",
                ConcurrencyStamp = "e5f6a7b8-c9d0-8e9f-2a3b-4c5d6e7f8a9b",
                PasswordHash = "AQAAAAIAAYagAAAAEJ7VXmHqEwQ7RjKzNQ2P6buXXqhRXW8RMZJqU8K8VjEwGJN8K8VjEwGJN8K8VjEwGJN8K8VjEwGJN8K8VjEw=="
            }
        );

        // Seed UserRole (Admin User -> Admin Role)
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                RoleId = adminRoleId,
                UserId = adminUserId
            }
        );
    }

    private void ConfigureGlobalFilters(ModelBuilder modelBuilder)
    {
        // Soft delete global filter (apenas para entidades customizadas)
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .HasQueryFilter(GenerateIsActiveFilter(entityType.ClrType));
            }
        }
    }

    private static LambdaExpression GenerateIsActiveFilter(Type type)
    {
        var parameter = Expression.Parameter(type, "e");
        var property = Expression.Property(parameter, nameof(BaseEntity.IsActive));
        var condition = Expression.Equal(property, Expression.Constant(true));
        return Expression.Lambda(condition, parameter);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Atualizar timestamps automaticamente
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }
        
        // Timestamps para ApplicationUser
        foreach (var entry in ChangeTracker.Entries<ApplicationUser>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}

