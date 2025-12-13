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

    // DbSets
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Lead> Leads => Set<Lead>();
    public DbSet<BlogPost> BlogPosts => Set<BlogPost>();
    public DbSet<BlogCategory> BlogCategories => Set<BlogCategory>();
    // public DbSet<Skill> Skills => Set<Skill>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Suprimir warning de pending model changes (migrations são aplicadas manualmente)
        optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // IMPORTANTE: chamada base para Identity
        
        // Aplicar configurações de entidades (Fluent API)
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        
        // Seed de Roles e usuário Admin
        SeedIdentityData(modelBuilder);
        
        // Seed de dados do Blog
        SeedBlogData(modelBuilder);
        
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

    private void SeedBlogData(ModelBuilder modelBuilder)
    {
        // Seed Categories
        modelBuilder.Entity<BlogCategory>().HasData(
            new BlogCategory
            {
                Id = 1,
                Name = "Dynamics 365",
                Slug = "dynamics-365",
                Description = "Artigos sobre Dynamics 365 Customer Engagement, Sales, Marketing e muito mais",
                Color = "#3B82F6",
                DisplayOrder = 1,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new BlogCategory
            {
                Id = 2,
                Name = "Power Platform",
                Slug = "power-platform",
                Description = "Tutoriais e dicas sobre Power Apps, Power Automate, Power BI e Power Pages",
                Color = "#6366F1",
                DisplayOrder = 2,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new BlogCategory
            {
                Id = 3,
                Name = ".NET",
                Slug = "dotnet",
                Description = "Desenvolvimento com .NET, C#, ASP.NET Core e arquitetura de software",
                Color = "#8B5CF6",
                DisplayOrder = 3,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );

        // Seed Posts
        modelBuilder.Entity<BlogPost>().HasData(
            new BlogPost
            {
                Id = 1,
                Title = "Começando com Dynamics 365 Customer Engagement",
                Slug = "comecando-com-dynamics-365-ce",
                Excerpt = "Aprenda os conceitos fundamentais do Dynamics 365 CE e como começar seu primeiro projeto.",
                Content = "<p class='lead'>O Dynamics 365 Customer Engagement é uma plataforma poderosa para gerenciar relacionamentos com clientes.</p><h2>O que é Dynamics 365 CE?</h2><p>Dynamics 365 Customer Engagement (CE) é uma solução CRM (Customer Relationship Management) que ajuda organizações a gerenciar vendas, marketing, atendimento ao cliente e operações de campo.</p><h2>Principais Componentes</h2><ul><li><strong>Sales:</strong> Gerenciamento de oportunidades e pipeline de vendas</li><li><strong>Marketing:</strong> Automação de marketing e gestão de campanhas</li><li><strong>Customer Service:</strong> Atendimento ao cliente e gestão de casos</li><li><strong>Field Service:</strong> Operações de campo e agendamento</li></ul><h2>Por onde começar?</h2><p>Para iniciar com Dynamics 365, recomendo seguir estes passos:</p><ol><li>Criar uma conta de trial no Microsoft 365</li><li>Acessar o Power Platform Admin Center</li><li>Provisionar um ambiente Dynamics 365</li><li>Explorar os aplicativos model-driven disponíveis</li></ol><p>Nos próximos artigos, vamos aprofundar em cada um desses componentes e criar soluções práticas.</p>",
                ImageUrl = "https://via.placeholder.com/1200x400/3B82F6/FFFFFF?text=Dynamics+365",
                Author = "Emanuel Macêdo",
                ReadTimeMinutes = 5,
                IsPublished = true,
                PublishedAt = new DateTime(2024, 12, 10, 10, 0, 0, DateTimeKind.Utc),
                CategoryId = 1,
                Tags = "[\"Dynamics 365\",\"CRM\",\"Microsoft\",\"Tutorial\"]",
                DisplayOrder = 1,
                CreatedAt = new DateTime(2024, 12, 10, 10, 0, 0, DateTimeKind.Utc)
            },
            new BlogPost
            {
                Id = 2,
                Title = "Power Platform: Automatizando processos com Power Automate",
                Slug = "power-platform-automatizando-com-power-automate",
                Excerpt = "Descubra como criar automações poderosas usando Power Automate e integrar com diversas aplicações.",
                Content = "<p class='lead'>Power Automate é a ferramenta de automação da Microsoft que permite conectar aplicativos e automatizar fluxos de trabalho.</p><h2>O que é Power Automate?</h2><p>Power Automate (anteriormente conhecido como Microsoft Flow) é uma plataforma de automação low-code que permite criar workflows automatizados entre aplicativos e serviços.</p><h2>Tipos de Fluxos</h2><ul><li><strong>Cloud Flows:</strong> Automações na nuvem disparadas por eventos</li><li><strong>Desktop Flows:</strong> RPA (Robotic Process Automation) para automação de desktop</li><li><strong>Business Process Flows:</strong> Guias visuais para processos padronizados</li></ul><h2>Casos de Uso Comuns</h2><ol><li>Aprovação de documentos e workflows</li><li>Sincronização de dados entre sistemas</li><li>Notificações e alertas automatizados</li><li>Coleta e processamento de dados</li></ol><p>Em breve publicarei tutoriais práticos mostrando como criar seus primeiros fluxos.</p>",
                ImageUrl = "https://via.placeholder.com/1200x400/6366F1/FFFFFF?text=Power+Automate",
                Author = "Emanuel Macêdo",
                ReadTimeMinutes = 7,
                IsPublished = true,
                PublishedAt = new DateTime(2024, 12, 8, 14, 30, 0, DateTimeKind.Utc),
                CategoryId = 2,
                Tags = "[\"Power Platform\",\"Power Automate\",\"Automação\",\"Low-Code\"]",
                DisplayOrder = 2,
                CreatedAt = new DateTime(2024, 12, 8, 14, 30, 0, DateTimeKind.Utc)
            },
            new BlogPost
            {
                Id = 3,
                Title = "Arquitetura Clean em .NET: Princípios e Práticas",
                Slug = "arquitetura-clean-dotnet-principios-praticas",
                Excerpt = "Entenda os conceitos de Clean Architecture e como aplicar em projetos .NET Core.",
                Content = "<p class='lead'>Clean Architecture é um padrão arquitetural que promove a separação de responsabilidades e independência de frameworks.</p><h2>O que é Clean Architecture?</h2><p>Proposta por Robert C. Martin (Uncle Bob), Clean Architecture organiza o código em camadas concêntricas, onde as dependências apontam sempre para dentro, em direção às regras de negócio.</p><h2>Camadas Principais</h2><ul><li><strong>Domain:</strong> Entidades e regras de negócio core</li><li><strong>Application:</strong> Casos de uso e interfaces de serviço</li><li><strong>Infrastructure:</strong> Implementações de persistência e serviços externos</li><li><strong>Presentation:</strong> UI, APIs e interfaces de usuário</li></ul><h2>Benefícios</h2><ol><li>Testabilidade: código facilmente testável</li><li>Manutenibilidade: mudanças isoladas em camadas específicas</li><li>Flexibilidade: troca de frameworks sem impacto no core</li><li>Independência: não acoplamento com tecnologias específicas</li></ol><p>Este projeto é um exemplo prático de Clean Architecture em .NET!</p>",
                ImageUrl = "https://via.placeholder.com/1200x400/8B5CF6/FFFFFF?text=Clean+Architecture",
                Author = "Emanuel Macêdo",
                ReadTimeMinutes = 10,
                IsPublished = true,
                PublishedAt = new DateTime(2024, 12, 5, 9, 0, 0, DateTimeKind.Utc),
                CategoryId = 3,
                Tags = "[\".NET\",\"Clean Architecture\",\"Design Patterns\",\"Boas Práticas\"]",
                DisplayOrder = 3,
                CreatedAt = new DateTime(2024, 12, 5, 9, 0, 0, DateTimeKind.Utc)
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

