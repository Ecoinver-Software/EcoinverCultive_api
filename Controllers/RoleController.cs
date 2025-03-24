using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EcoinverGMAO_api.Models.Dto;
using EcoinverGMAO_api.Services;

namespace EcoinverGMAO_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RolesController> _logger;

        public RolesController(IRoleService roleService, ILogger<RolesController> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles.Select(r => new
            {
                r.Id,
                r.Name,
                r.Description,
                r.Level
            }));
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoleDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await _roleService.CreateRoleAsync(dto);
                return Ok(new { message = $"Rol '{dto.Name}' creado exitosamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear rol");
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var role = await _roleService.GetRoleByIdAsync(id);
                return Ok(new
                {
                    role.Id,
                    role.Name,
                    role.Description,
                    role.Level
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error al obtener rol");
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateRoleDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await _roleService.UpdateRoleAsync(id, dto);
                return Ok(new { message = "Rol actualizado exitosamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar rol");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _roleService.DeleteRoleAsync(id);
                return Ok(new { message = "Rol eliminado exitosamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar rol");
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
