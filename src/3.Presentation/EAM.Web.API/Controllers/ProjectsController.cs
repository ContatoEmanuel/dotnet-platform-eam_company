using Microsoft.AspNetCore.Mvc;
using EAM.Core.Application.DTOs.Project;
using EAM.Core.Application.Services.Interfaces;

namespace EAM.Web.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly ILogger<ProjectsController> _logger;

    public ProjectsController(IProjectService projectService, ILogger<ProjectsController> logger)
    {
        _projectService = projectService;
        _logger = logger;
    }

    /// <summary>
    /// Retorna todos os projetos
    /// </summary>
    /// <returns>Lista de projetos</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProjectDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAll([FromQuery] string? language = "pt-BR")
    {
        try
        {
            var projects = await _projectService.GetAllProjectsAsync();
            return Ok(projects);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar todos os projetos");
            return StatusCode(500, new { message = "Erro interno ao processar a requisição" });
        }
    }

    /// <summary>
    /// Retorna apenas projetos em destaque
    /// </summary>
    /// <returns>Lista de projetos em destaque</returns>
    [HttpGet("featured")]
    [ProducesResponseType(typeof(IEnumerable<ProjectDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetFeatured([FromQuery] string? language = "pt-BR")
    {
        try
        {
            var projects = await _projectService.GetFeaturedProjectsAsync();
            return Ok(projects);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar projetos em destaque");
            return StatusCode(500, new { message = "Erro interno ao processar a requisição" });
        }
    }

    /// <summary>
    /// Retorna outros projetos (não destacados)
    /// </summary>
    /// <returns>Lista de outros projetos</returns>
    [HttpGet("other")]
    [ProducesResponseType(typeof(IEnumerable<ProjectDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetOther([FromQuery] string? language = "pt-BR")
    {
        try
        {
            var projects = await _projectService.GetOtherProjectsAsync();
            return Ok(projects);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar outros projetos");
            return StatusCode(500, new { message = "Erro interno ao processar a requisição" });
        }
    }

    /// <summary>
    /// Retorna um projeto específico por ID
    /// </summary>
    /// <param name="id">ID do projeto</param>
    /// <returns>Dados do projeto</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProjectDto>> GetById(int id, [FromQuery] string? language = "pt-BR")
    {
        try
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            
            if (project == null)
            {
                return NotFound(new { message = $"Projeto com ID {id} não encontrado" });
            }

            return Ok(project);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar projeto {ProjectId}", id);
            return StatusCode(500, new { message = "Erro interno ao processar a requisição" });
        }
    }

    /// <summary>
    /// Cria um novo projeto
    /// </summary>
    /// <param name="projectDto">Dados do projeto</param>
    /// <returns>Projeto criado</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProjectDto>> Create([FromBody] ProjectCreateDto projectDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdProject = await _projectService.CreateProjectAsync(projectDto);
            
            return CreatedAtAction(
                nameof(GetById), 
                new { id = createdProject.Id }, 
                createdProject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar projeto");
            return StatusCode(500, new { message = "Erro interno ao processar a requisição" });
        }
    }

    /// <summary>
    /// Atualiza um projeto existente
    /// </summary>
    /// <param name="id">ID do projeto</param>
    /// <param name="projectDto">Dados atualizados</param>
    /// <returns>Projeto atualizado</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProjectDto>> Update(int id, [FromBody] ProjectCreateDto projectDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedProject = await _projectService.UpdateProjectAsync(id, projectDto);
            
            if (updatedProject == null)
            {
                return NotFound(new { message = $"Projeto com ID {id} não encontrado" });
            }

            return Ok(updatedProject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar projeto {ProjectId}", id);
            return StatusCode(500, new { message = "Erro interno ao processar a requisição" });
        }
    }

    /// <summary>
    /// Remove um projeto
    /// </summary>
    /// <param name="id">ID do projeto</param>
    /// <returns>Confirmação de exclusão</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _projectService.DeleteProjectAsync(id);
            
            if (!result)
            {
                return NotFound(new { message = $"Projeto com ID {id} não encontrado" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir projeto {ProjectId}", id);
            return StatusCode(500, new { message = "Erro interno ao processar a requisição" });
        }
    }
}
