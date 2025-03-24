using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcoinverGMAO_api.Models.Dto;
using EcoinverGMAO_api.Models.Identity;
using EcoinverGMAO_api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EcoinverGMAO_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            IUserService userService,
            UserManager<User> userManager,
            ILogger<UsersController> logger)
        {
            _userService = userService;
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene la lista de usuarios, mostrando solo DTOs (sin exponer contraseñas ni hashes).
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                // Como solo hay un rol por usuario, tomamos el primero de la lista.
                var roles = await _userManager.GetRolesAsync(user);
                var singleRole = roles.FirstOrDefault() ?? string.Empty;

                userDtos.Add(new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    NombreCompleto = user.NombreCompleto,
                    Role = singleRole
                });
            }

            return Ok(userDtos);
        }

        /// <summary>
        /// Crea un usuario nuevo con un rol único.
        /// </summary>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateUserDto createUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await _userService.CreateUserAsync(createUserDto);

                // Mapear a UserDto para devolverlo
                var userDto = new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    NombreCompleto = user.NombreCompleto,
                    // Si quieres confirmar el rol asignado, podrías volver a pedirlo:
                    Role = string.IsNullOrEmpty(createUserDto.Role)
                                ? string.Empty
                                : createUserDto.Role
                };

                return Ok(new
                {
                    message = "Usuario creado exitosamente.",
                    user = userDto
                });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error al crear usuario");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un usuario por Id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);

                var roles = await _userManager.GetRolesAsync(user);
                var singleRole = roles.FirstOrDefault() ?? string.Empty;

                var userDto = new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    NombreCompleto = user.NombreCompleto,
                    Role = singleRole
                };

                return Ok(userDto);
            }
            catch (System.Exception ex)
            {
                _logger.LogWarning(ex, "Error al obtener usuario por ID");
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza un usuario existente (email, nombre, contraseña, rol único).
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateUserDto updateUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await _userService.UpdateUserAsync(id, updateUserDto);

                // Obtenemos el rol único actualizado
                var roles = await _userManager.GetRolesAsync(user);
                var singleRole = roles.FirstOrDefault() ?? string.Empty;

                var userDto = new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    NombreCompleto = user.NombreCompleto,
                    Role = singleRole
                };

                return Ok(new
                {
                    message = "Usuario actualizado exitosamente.",
                    user = userDto
                });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar usuario");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Elimina un usuario por Id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return Ok(new { message = "Usuario eliminado exitosamente." });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar usuario");
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
