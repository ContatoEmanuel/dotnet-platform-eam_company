using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EAM.Infra.Data.Context;
using EAM.Core.Domain.Entities;

namespace EAM.Web.API.Controllers;

/// <summary>
/// Controller para gerenciar traduções e configurações de idioma.
/// Endpoints administrativos para Setup de múltiplos idiomas.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Tags("Admin - Languages")]
public class LanguagesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<LanguagesController> _logger;

    public LanguagesController(ApplicationDbContext context, ILogger<LanguagesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Inicializa o setup de múltiplos idiomas.
    /// Adiciona traduções para categorias de blog.
    /// </summary>
    /// <remarks>
    /// Este endpoint configura as categorias com suas traduções em inglês.
    /// Segue a arquitetura do projeto: Web.Public → Web.API → Database
    /// 
    /// Categorias traduzidas:
    /// - Dynamics 365 → Dynamics 365
    /// - Power Platform → Power Platform
    /// - .NET → DotNet
    /// </remarks>
    [HttpPost("setup-translations")]
    [ProducesResponseType(typeof(SetupTranslationsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SetupTranslations()
    {
        try
        {
            _logger.LogInformation("Iniciando setup de traduções...");

            var categoriesUpdated = 0;

            // Dynamics 365
            var cat1 = await _context.BlogCategories.FirstOrDefaultAsync(c => c.Name == "Dynamics 365");
            if (cat1 != null && string.IsNullOrEmpty(cat1.NameEn))
            {
                cat1.NameEn = "Dynamics 365";
                cat1.SlugEn = "dynamics-365";
                cat1.DescriptionEn = "Articles about Microsoft Dynamics 365 CRM - Sales, Marketing, and Customer Engagement";
                categoriesUpdated++;
                _logger.LogInformation("Tradução de 'Dynamics 365' adicionada");
            }

            // Power Platform
            var cat2 = await _context.BlogCategories.FirstOrDefaultAsync(c => c.Name == "Power Platform");
            if (cat2 != null && string.IsNullOrEmpty(cat2.NameEn))
            {
                cat2.NameEn = "Power Platform";
                cat2.SlugEn = "power-platform";
                cat2.DescriptionEn = "Tutorials and tips about Microsoft Power Platform - Power Apps, Power Automate, Power BI and Power Pages";
                categoriesUpdated++;
                _logger.LogInformation("Tradução de 'Power Platform' adicionada");
            }

            // .NET
            var cat3 = await _context.BlogCategories.FirstOrDefaultAsync(c => c.Name == ".NET");
            if (cat3 != null && string.IsNullOrEmpty(cat3.NameEn))
            {
                cat3.NameEn = "DotNet";
                cat3.SlugEn = "dotnet";
                cat3.DescriptionEn = "Development with .NET, C#, ASP.NET Core and software architecture";
                categoriesUpdated++;
                _logger.LogInformation("Tradução de '.NET' adicionada");
            }

            // Salvar alterações
            await _context.SaveChangesAsync();

            // Obter resumo
            var totalCategories = await _context.BlogCategories.CountAsync();
            var totalPosts = await _context.BlogPosts.CountAsync();
            var totalProjects = await _context.Projects.CountAsync();

            var translatedCategories = await _context.BlogCategories.CountAsync(c => c.NameEn != null);
            var translatedPosts = await _context.BlogPosts.CountAsync(p => p.TitleEn != null);
            var translatedProjects = await _context.Projects.CountAsync(p => p.TitleEn != null);

            var response = new SetupTranslationsResponse
            {
                Success = true,
                Message = $"Setup de traduções concluído com sucesso! {categoriesUpdated} categorias atualizadas.",
                Data = new TranslationsStatus
                {
                    Categories = new EntityTranslationStatus
                    {
                        Total = totalCategories,
                        Translated = translatedCategories,
                        Pending = totalCategories - translatedCategories
                    },
                    Posts = new EntityTranslationStatus
                    {
                        Total = totalPosts,
                        Translated = translatedPosts,
                        Pending = totalPosts - translatedPosts
                    },
                    Projects = new EntityTranslationStatus
                    {
                        Total = totalProjects,
                        Translated = translatedProjects,
                        Pending = totalProjects - translatedProjects
                    }
                }
            };

            _logger.LogInformation("Setup de traduções concluído com sucesso");
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao executar setup de traduções");
            return StatusCode(500, new { message = "Erro ao configurar traduções", error = ex.Message });
        }
    }

    /// <summary>
    /// Adiciona tradução para um post de blog.
    /// </summary>
    [HttpPost("posts/{id}/translate")]
    [ProducesResponseType(typeof(TranslatePostResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> TranslatePost(int id, [FromBody] PostTranslationRequest request)
    {
        if (request == null)
            return BadRequest(new { message = "Corpo da requisição não pode ser vazio" });

        try
        {
            var post = await _context.BlogPosts.FirstOrDefaultAsync(p => p.Id == id);
            if (post == null)
                return NotFound(new { message = $"Post com ID {id} não encontrado" });

            post.TitleEn = request.TitleEn;
            post.SlugEn = request.SlugEn;
            post.ExcerptEn = request.ExcerptEn;
            post.ContentEn = request.ContentEn;

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Post ID {id} traduzido com sucesso");

            return Ok(new TranslatePostResponse
            {
                Success = true,
                Message = $"Post ID {id} traduzido com sucesso",
                Post = new PostTranslationData
                {
                    Id = post.Id,
                    Title = post.Title,
                    TitleEn = post.TitleEn,
                    HasEnglishTranslation = !string.IsNullOrEmpty(post.TitleEn)
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao traduzir post ID {id}");
            return StatusCode(500, new { message = "Erro ao traduzir post", error = ex.Message });
        }
    }

    /// <summary>
    /// Adiciona tradução para um projeto.
    /// </summary>
    [HttpPost("projects/{id}/translate")]
    [ProducesResponseType(typeof(TranslateProjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> TranslateProject(int id, [FromBody] ProjectTranslationRequest request)
    {
        if (request == null)
            return BadRequest(new { message = "Corpo da requisição não pode ser vazio" });

        try
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
            if (project == null)
                return NotFound(new { message = $"Projeto com ID {id} não encontrado" });

            project.TitleEn = request.TitleEn;
            project.DescriptionEn = request.DescriptionEn;

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Projeto ID {id} traduzido com sucesso");

            return Ok(new TranslateProjectResponse
            {
                Success = true,
                Message = $"Projeto ID {id} traduzido com sucesso",
                Project = new ProjectTranslationData
                {
                    Id = project.Id,
                    Title = project.Title,
                    TitleEn = project.TitleEn,
                    HasEnglishTranslation = !string.IsNullOrEmpty(project.TitleEn)
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao traduzir projeto ID {id}");
            return StatusCode(500, new { message = "Erro ao traduzir projeto", error = ex.Message });
        }
    }

    /// <summary>
    /// Obtém o status de traduções do sistema.
    /// </summary>
    [HttpGet("status")]
    [ProducesResponseType(typeof(TranslationsStatusResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTranslationsStatus()
    {
        try
        {
            var totalCategories = await _context.BlogCategories.CountAsync();
            var totalPosts = await _context.BlogPosts.CountAsync();
            var totalProjects = await _context.Projects.CountAsync();

            var translatedCategories = await _context.BlogCategories.CountAsync(c => c.NameEn != null);
            var translatedPosts = await _context.BlogPosts.CountAsync(p => p.TitleEn != null);
            var translatedProjects = await _context.Projects.CountAsync(p => p.TitleEn != null);

            var response = new TranslationsStatusResponse
            {
                Categories = new EntityTranslationStatus
                {
                    Total = totalCategories,
                    Translated = translatedCategories,
                    Pending = totalCategories - translatedCategories
                },
                Posts = new EntityTranslationStatus
                {
                    Total = totalPosts,
                    Translated = translatedPosts,
                    Pending = totalPosts - translatedPosts
                },
                Projects = new EntityTranslationStatus
                {
                    Total = totalProjects,
                    Translated = translatedProjects,
                    Pending = totalProjects - translatedProjects
                }
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter status de traduções");
            return StatusCode(500, new { message = "Erro ao obter status", error = ex.Message });
        }
    }

    /// <summary>
    /// Obtém lista de posts que precisam tradução.
    /// </summary>
    [HttpGet("posts/pending")]
    [ProducesResponseType(typeof(PendingPostsResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPendingPosts()
    {
        try
        {
            var pendingPosts = await _context.BlogPosts
                .Where(p => p.TitleEn == null || p.TitleEn == "")
                .Select(p => new PendingPost
                {
                    Id = p.Id,
                    Title = p.Title,
                    Excerpt = p.Excerpt
                })
                .ToListAsync();

            return Ok(new PendingPostsResponse
            {
                Total = pendingPosts.Count,
                Posts = pendingPosts
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter posts pendentes");
            return StatusCode(500, new { message = "Erro ao obter posts", error = ex.Message });
        }
    }

    /// <summary>
    /// Obtém lista de projetos que precisam tradução.
    /// </summary>
    [HttpGet("projects/pending")]
    [ProducesResponseType(typeof(PendingProjectsResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPendingProjects()
    {
        try
        {
            var pendingProjects = await _context.Projects
                .Where(p => p.TitleEn == null || p.TitleEn == "")
                .Select(p => new PendingProject
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description
                })
                .ToListAsync();

            return Ok(new PendingProjectsResponse
            {
                Total = pendingProjects.Count,
                Projects = pendingProjects
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter projetos pendentes");
            return StatusCode(500, new { message = "Erro ao obter projetos", error = ex.Message });
        }
    }
}

// DTOs para requisições
public class PostTranslationRequest
{
    public string TitleEn { get; set; } = string.Empty;
    public string SlugEn { get; set; } = string.Empty;
    public string ExcerptEn { get; set; } = string.Empty;
    public string ContentEn { get; set; } = string.Empty;
}

public class ProjectTranslationRequest
{
    public string TitleEn { get; set; } = string.Empty;
    public string DescriptionEn { get; set; } = string.Empty;
}

// DTOs para respostas
public class SetupTranslationsResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public TranslationsStatus? Data { get; set; }
}

public class TranslationsStatus
{
    public EntityTranslationStatus Categories { get; set; } = new();
    public EntityTranslationStatus Posts { get; set; } = new();
    public EntityTranslationStatus Projects { get; set; } = new();
}

public class TranslationsStatusResponse
{
    public EntityTranslationStatus Categories { get; set; } = new();
    public EntityTranslationStatus Posts { get; set; } = new();
    public EntityTranslationStatus Projects { get; set; } = new();
}

public class EntityTranslationStatus
{
    public int Total { get; set; }
    public int Translated { get; set; }
    public int Pending { get; set; }
}

public class TranslatePostResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public PostTranslationData? Post { get; set; }
}

public class PostTranslationData
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? TitleEn { get; set; }
    public bool HasEnglishTranslation { get; set; }
}

public class TranslateProjectResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public ProjectTranslationData? Project { get; set; }
}

public class ProjectTranslationData
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? TitleEn { get; set; }
    public bool HasEnglishTranslation { get; set; }
}

public class PendingPostsResponse
{
    public int Total { get; set; }
    public List<PendingPost> Posts { get; set; } = new();
}

public class PendingPost
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Excerpt { get; set; } = string.Empty;
}

public class PendingProjectsResponse
{
    public int Total { get; set; }
    public List<PendingProject> Projects { get; set; } = new();
}

public class PendingProject
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
}
