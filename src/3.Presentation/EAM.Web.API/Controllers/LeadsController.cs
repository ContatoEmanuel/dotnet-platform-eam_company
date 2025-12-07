using Microsoft.AspNetCore.Mvc;
using EAM.Core.Application.DTOs.Lead;
using EAM.Core.Domain.Entities;
using EAM.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace EAM.Web.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class LeadsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<LeadsController> _logger;

    public LeadsController(ApplicationDbContext context, ILogger<LeadsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Retorna todos os leads
    /// </summary>
    /// <returns>Lista de leads</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<LeadDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<LeadDto>>> GetAll()
    {
        try
        {
            var leads = await _context.Leads
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();

            var leadsDto = leads.Select(l => new LeadDto
            {
                Id = l.Id,
                FullName = l.FullName,
                Email = l.Email,
                Phone = l.Phone,
                Company = l.Company,
                Message = l.Message,
                Subject = l.Subject,
                CreatedAt = l.CreatedAt,
                IsContacted = l.IsContacted,
                ContactedAt = l.ContactedAt
            });

            return Ok(leadsDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar leads");
            return StatusCode(500, new { message = "Erro interno ao processar a requisição" });
        }
    }

    /// <summary>
    /// Retorna um lead específico por ID
    /// </summary>
    /// <param name="id">ID do lead</param>
    /// <returns>Dados do lead</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(LeadDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LeadDto>> GetById(int id)
    {
        try
        {
            var lead = await _context.Leads.FindAsync(id);
            
            if (lead == null)
            {
                return NotFound(new { message = $"Lead com ID {id} não encontrado" });
            }

            var leadDto = new LeadDto
            {
                Id = lead.Id,
                FullName = lead.FullName,
                Email = lead.Email,
                Phone = lead.Phone,
                Company = lead.Company,
                Message = lead.Message,
                Subject = lead.Subject,
                CreatedAt = lead.CreatedAt,
                IsContacted = lead.IsContacted,
                ContactedAt = lead.ContactedAt
            };

            return Ok(leadDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar lead {LeadId}", id);
            return StatusCode(500, new { message = "Erro interno ao processar a requisição" });
        }
    }

    /// <summary>
    /// Cria um novo lead (formulário de contato)
    /// </summary>
    /// <param name="leadDto">Dados do lead</param>
    /// <returns>Lead criado</returns>
    [HttpPost]
    [ProducesResponseType(typeof(LeadDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LeadDto>> Create([FromBody] LeadCreateDto leadDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var lead = new Lead
            {
                FullName = leadDto.FullName,
                Email = leadDto.Email,
                Phone = leadDto.Phone,
                Company = leadDto.Company,
                Message = leadDto.Message,
                Subject = leadDto.Subject,
                CreatedAt = DateTime.UtcNow
            };

            _context.Leads.Add(lead);
            await _context.SaveChangesAsync();

            var createdLeadDto = new LeadDto
            {
                Id = lead.Id,
                FullName = lead.FullName,
                Email = lead.Email,
                Phone = lead.Phone,
                Company = lead.Company,
                Message = lead.Message,
                Subject = lead.Subject,
                CreatedAt = lead.CreatedAt,
                IsContacted = lead.IsContacted,
                ContactedAt = lead.ContactedAt
            };

            _logger.LogInformation("Novo lead criado: {LeadEmail}", lead.Email);

            return CreatedAtAction(nameof(GetById), new { id = lead.Id }, createdLeadDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar lead");
            return StatusCode(500, new { message = "Erro interno ao processar a requisição" });
        }
    }

    /// <summary>
    /// Marca um lead como contatado
    /// </summary>
    /// <param name="id">ID do lead</param>
    /// <returns>Lead atualizado</returns>
    [HttpPatch("{id}/contact")]
    [ProducesResponseType(typeof(LeadDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LeadDto>> MarkAsContacted(int id)
    {
        try
        {
            var lead = await _context.Leads.FindAsync(id);
            
            if (lead == null)
            {
                return NotFound(new { message = $"Lead com ID {id} não encontrado" });
            }

            lead.IsContacted = true;
            lead.ContactedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var leadDto = new LeadDto
            {
                Id = lead.Id,
                FullName = lead.FullName,
                Email = lead.Email,
                Phone = lead.Phone,
                Company = lead.Company,
                Message = lead.Message,
                Subject = lead.Subject,
                CreatedAt = lead.CreatedAt,
                IsContacted = lead.IsContacted,
                ContactedAt = lead.ContactedAt
            };

            return Ok(leadDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao marcar lead como contatado {LeadId}", id);
            return StatusCode(500, new { message = "Erro interno ao processar a requisição" });
        }
    }

    /// <summary>
    /// Remove um lead
    /// </summary>
    /// <param name="id">ID do lead</param>
    /// <returns>Confirmação de exclusão</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var lead = await _context.Leads.FindAsync(id);
            
            if (lead == null)
            {
                return NotFound(new { message = $"Lead com ID {id} não encontrado" });
            }

            _context.Leads.Remove(lead);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir lead {LeadId}", id);
            return StatusCode(500, new { message = "Erro interno ao processar a requisição" });
        }
    }
}
