using EAM.Core.Domain.Entities;
using EAM.Infra.Data.Context;
using System.Text.Json;

namespace EAM.Web.Public.Data;

public static class DbInitializer
{
    public static async Task SeedProjectsAsync(ApplicationDbContext context)
    {
        // Verifica se já existem projetos
        if (context.Projects.Any())
        {
            return; // Já foi populado
        }

        var projects = new List<Project>
        {
            // Projeto 1: Dynamics 365 WhatsApp Connector (Destaque)
            new Project
            {
                Title = "Dynamics 365 WhatsApp Connector",
                Description = "Extensão Chrome para integração automática entre WhatsApp Web e Microsoft Dynamics 365. Permite criar contatos e leads diretamente do WhatsApp com autenticação Azure AD.",
                Technologies = JsonSerializer.Serialize(new[] { "TypeScript", "Chrome Extension", "Azure AD", "Dynamics 365", "React" }),
                GithubUrl = "https://github.com/ContatoEmanuel/react-extension-dynamics_whatsapp",
                IconEmoji = "💬",
                IsFeatured = true,
                DisplayOrder = 1,
                CreatedAt = DateTime.UtcNow
            },
            
            // Projeto 2: .NET Platform EAM Company (Destaque)
            new Project
            {
                Title = ".NET Platform EAM Company",
                Description = "Plataforma corporativa multi-projeto com Clean Architecture, Docker, API REST, site institucional MVC e portal Blazor. Configuração completa para ambientes Linux.",
                Technologies = JsonSerializer.Serialize(new[] { "ASP.NET Core", "Docker", "TypeScript", "Blazor", "Clean Architecture" }),
                GithubUrl = "https://github.com/ContatoEmanuel/dotnet-platform-eam_company",
                IconEmoji = "🏗️",
                IsFeatured = true,
                DisplayOrder = 2,
                CreatedAt = DateTime.UtcNow
            },
            
            // Projeto 3: React Portfolio Website (Destaque)
            new Project
            {
                Title = "React Portfolio Website",
                Description = "Website profissional desenvolvido com React, TypeScript e Tailwind CSS. Deploy automatizado na Vercel com domínio customizado e HTTPS.",
                Technologies = JsonSerializer.Serialize(new[] { "React", "TypeScript", "Tailwind CSS", "Vite", "Vercel" }),
                LiveUrl = "https://eam-company.com.br",
                GithubUrl = "https://github.com/ContatoEmanuel/react-front-portfolio",
                IconEmoji = "🌐",
                IsFeatured = true,
                DisplayOrder = 3,
                CreatedAt = DateTime.UtcNow
            },
            
            // Projeto 4: Dynamics 365 Custom Plugins
            new Project
            {
                Title = "Dynamics 365 Custom Plugins",
                Description = "Coleção de plugins customizados para Dynamics 365, incluindo validações complexas, integrações com APIs externas e automações de processos de negócio.",
                Technologies = JsonSerializer.Serialize(new[] { "C# .NET", "Dynamics 365" }),
                IconEmoji = "🔌",
                IsFeatured = false,
                DisplayOrder = 4,
                CreatedAt = DateTime.UtcNow
            },
            
            // Projeto 5: Power Platform Solutions
            new Project
            {
                Title = "Power Platform Solutions",
                Description = "Soluções enterprise desenvolvidas com Power Apps (Canvas e Model-Driven), Power Automate para automação de processos e Power BI para dashboards executivos.",
                Technologies = JsonSerializer.Serialize(new[] { "Power Apps", "Power Automate" }),
                IconEmoji = "⚡",
                IsFeatured = false,
                DisplayOrder = 5,
                CreatedAt = DateTime.UtcNow
            },
            
            // Projeto 6: Azure Integration Services
            new Project
            {
                Title = "Azure Integration Services",
                Description = "Implementação de soluções de integração na nuvem utilizando Azure Logic Apps, Azure Data Factory e Azure Functions para orquestração de processos empresariais.",
                Technologies = JsonSerializer.Serialize(new[] { "Azure Logic Apps", "Data Factory" }),
                IconEmoji = "☁️",
                IsFeatured = false,
                DisplayOrder = 6,
                CreatedAt = DateTime.UtcNow
            },
            
            // Projeto 7: CRM Analytics Dashboard
            new Project
            {
                Title = "CRM Analytics Dashboard",
                Description = "Dashboard interativo desenvolvido em Power BI com integração ao Dynamics 365, fornecendo insights de vendas, atendimento e performance de equipes.",
                Technologies = JsonSerializer.Serialize(new[] { "Power BI", "Dynamics 365" }),
                IconEmoji = "📊",
                IsFeatured = false,
                DisplayOrder = 7,
                CreatedAt = DateTime.UtcNow
            }
        };

        await context.Projects.AddRangeAsync(projects);
        await context.SaveChangesAsync();
    }
}
