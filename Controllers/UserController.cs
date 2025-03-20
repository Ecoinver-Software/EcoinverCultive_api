using System.Linq;
using System.Threading.Tasks;
using EcoinverGMAO_api.Models.Dto;
using EcoinverGMAO_api.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EcoinverGMAO_api.Controllers
{
    [ApiController]
    [Route("api/users")]
    // [Authorize]

    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            ILogger<UsersController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }
        // 4) GET: api/users
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userManager.Users.ToList();
            return Ok(users);
        }
        // 1) POST: api/users/create
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateUserDto createUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verifica si ya existe un usuario con ese nombre
            var existingUser = await _userManager.FindByNameAsync(createUserDto.Username);
            if (existingUser != null)
            {
                return BadRequest(new { message = "El usuario ya existe." });
            }

            var newUser = new User
            {
                UserName = createUserDto.Username,
                Email = createUserDto.Email,
                NombreCompleto = createUserDto.NombreCompleto,
                EmailConfirmed = true
            };

            // Crea el usuario con la contraseña proporcionada
            var createResult = await _userManager.CreateAsync(newUser, createUserDto.Password);
            if (!createResult.Succeeded)
            {
                _logger.LogError("Error al crear el usuario: {Errors}", string.Join(", ", createResult.Errors.Select(e => e.Description)));
                return BadRequest(createResult.Errors);
            }

            // Asigna el rol único al usuario
            if (!string.IsNullOrEmpty(createUserDto.Role))
            {
                if (await _roleManager.RoleExistsAsync(createUserDto.Role))
                {
                    var addRoleResult = await _userManager.AddToRoleAsync(newUser, createUserDto.Role);
                    if (!addRoleResult.Succeeded)
                    {
                        _logger.LogError("Error al asignar el rol {Role} al usuario {User}: {Errors}",
                            createUserDto.Role, newUser.UserName,
                            string.Join(", ", addRoleResult.Errors.Select(e => e.Description)));
                        return BadRequest(addRoleResult.Errors);
                    }
                }
                else
                {
                    return BadRequest(new { message = $"El rol '{createUserDto.Role}' no existe." });
                }
            }

            return Ok(new { message = "Usuario creado exitosamente." });
        }
        // 5) GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "Usuario no encontrado." });
            }
            return Ok(user);
        }
        // 2) PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateUserDto updateUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "Usuario no encontrado." });
            }

            // Actualiza los campos permitidos
            user.Email = updateUserDto.Email ?? user.Email;
            user.NombreCompleto = updateUserDto.NombreCompleto ?? user.NombreCompleto;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                _logger.LogError("Error al actualizar el usuario: {Errors}",
                    string.Join(", ", updateResult.Errors.Select(e => e.Description)));
                return BadRequest(updateResult.Errors);
            }

            // Actualiza la contraseña si se ha enviado
            if (!string.IsNullOrEmpty(updateUserDto.Password))
            {
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordResult = await _userManager.ResetPasswordAsync(user, resetToken, updateUserDto.Password);
                if (!passwordResult.Succeeded)
                {
                    _logger.LogError("Error al actualizar la contraseña: {Errors}",
                        string.Join(", ", passwordResult.Errors.Select(e => e.Description)));
                    return BadRequest(passwordResult.Errors);
                }
            }

            // Actualiza el rol si se especifica
            if (!string.IsNullOrEmpty(updateUserDto.Role))
            {
                // Obtiene el rol actual (se asume que el usuario solo tiene uno)
                var currentRoles = await _userManager.GetRolesAsync(user);
                // Elimina el rol actual, si existe
                if (currentRoles.Any())
                {
                    var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    if (!removeResult.Succeeded)
                    {
                        _logger.LogError("Error al eliminar roles actuales del usuario {User}: {Errors}",
                            user.UserName, string.Join(", ", removeResult.Errors.Select(e => e.Description)));
                        return BadRequest(removeResult.Errors);
                    }
                }
                // Asigna el nuevo rol
                if (await _roleManager.RoleExistsAsync(updateUserDto.Role))
                {
                    var addRoleResult = await _userManager.AddToRoleAsync(user, updateUserDto.Role);
                    if (!addRoleResult.Succeeded)
                    {
                        _logger.LogError("Error al asignar el nuevo rol {Role} al usuario {User}: {Errors}",
                            updateUserDto.Role, user.UserName,
                            string.Join(", ", addRoleResult.Errors.Select(e => e.Description)));
                        return BadRequest(addRoleResult.Errors);
                    }
                }
                else
                {
                    return BadRequest(new { message = $"El rol '{updateUserDto.Role}' no existe." });
                }
            }

            return Ok(new { message = "Usuario actualizado exitosamente." });
        }

        // 3) DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "Usuario no encontrado." });
            }

            var deleteResult = await _userManager.DeleteAsync(user);
            if (!deleteResult.Succeeded)
            {
                _logger.LogError("Error al eliminar el usuario: {Errors}",
                    string.Join(", ", deleteResult.Errors.Select(e => e.Description)));
                return BadRequest(deleteResult.Errors);
            }

            return Ok(new { message = "Usuario eliminado exitosamente." });
        }
    }
}
