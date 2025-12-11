using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EAM.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EAM.Web.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<UsersController> _logger;

    public UsersController(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ILogger<UsersController> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    /// <summary>
    /// Lista todos os usuários com suas roles
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var users = await _userManager.Users.ToListAsync();
            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDtos.Add(new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    EmailConfirmed = user.EmailConfirmed,
                    Roles = roles.ToList()
                });
            }

            return Ok(userDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar usuários");
            return StatusCode(500, new { message = "Erro interno no servidor" });
        }
    }

    /// <summary>
    /// Obtém um usuário por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound(new { message = "Usuário não encontrado" });

            var roles = await _userManager.GetRolesAsync(user);
            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                EmailConfirmed = user.EmailConfirmed,
                Roles = roles.ToList()
            };

            return Ok(userDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter usuário");
            return StatusCode(500, new { message = "Erro interno no servidor" });
        }
    }

    /// <summary>
    /// Adiciona role Admin a um usuário
    /// </summary>
    [HttpPost("{id}/roles/admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddAdminRole(string id)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound(new { message = "Usuário não encontrado" });

            // Garantir que a role existe
            if (!await _roleManager.RoleExistsAsync("Admin"))
                await _roleManager.CreateAsync(new IdentityRole("Admin"));

            var result = await _userManager.AddToRoleAsync(user, "Admin");
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest(new { message = $"Erro ao adicionar role: {errors}" });
            }

            return Ok(new { message = $"Usuário {user.Email} promovido a Administrador" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao adicionar role admin");
            return StatusCode(500, new { message = "Erro interno no servidor" });
        }
    }

    /// <summary>
    /// Remove role Admin de um usuário
    /// </summary>
    [HttpDelete("{id}/roles/admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveAdminRole(string id)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound(new { message = "Usuário não encontrado" });

            // Verificar se não é o último admin
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            if (admins.Count <= 1)
                return BadRequest(new { message = "Não é possível remover o último administrador" });

            var result = await _userManager.RemoveFromRoleAsync(user, "Admin");
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest(new { message = $"Erro ao remover role: {errors}" });
            }

            return Ok(new { message = $"Privilégios de administrador removidos de {user.Email}" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover role admin");
            return StatusCode(500, new { message = "Erro interno no servidor" });
        }
    }

    /// <summary>
    /// Cria um novo usuário
    /// </summary>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
                return BadRequest(new { message = "E-mail já cadastrado" });

            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest(new { message = errors });
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                Roles = new List<string>()
            };

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, userDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar usuário");
            return StatusCode(500, new { message = "Erro interno no servidor" });
        }
    }
}

public class UserDto
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool EmailConfirmed { get; set; }
    public List<string> Roles { get; set; } = new();
}

public class CreateUserDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
