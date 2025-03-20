using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcoinverGMAO_api.Models.Dto;
using EcoinverGMAO_api.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace EcoinverGMAO_api.Services
{
    public interface IUserService
    {
        Task CreateUserAsync(CreateUserDto dto);
        Task UpdateUserAsync(string id, UpdateUserDto dto);
        Task<User> GetUserByIdAsync(string id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task DeleteUserAsync(string id);
    }

    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public UserService(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task CreateUserAsync(CreateUserDto dto)
        {
            // Verificar si el usuario ya existe
            var existingUser = await _userManager.FindByNameAsync(dto.Username);
            if (existingUser != null)
            {
                throw new Exception("El usuario ya existe.");
            }

            var user = new User
            {
                UserName = dto.Username,
                Email = dto.Email,
                NombreCompleto = dto.NombreCompleto,
                EmailConfirmed = true
            };

            // Crear el usuario con la contraseña indicada
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                throw new Exception("Error al crear el usuario: " +
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            // Asignar el rol único si se especifica
            if (!string.IsNullOrEmpty(dto.Role))
            {
                if (await _roleManager.RoleExistsAsync(dto.Role))
                {
                    var addRoleResult = await _userManager.AddToRoleAsync(user, dto.Role);
                    if (!addRoleResult.Succeeded)
                    {
                        throw new Exception("Error al asignar el rol: " +
                            string.Join(", ", addRoleResult.Errors.Select(e => e.Description)));
                    }
                }
                else
                {
                    throw new Exception($"El rol '{dto.Role}' no existe.");
                }
            }
        }

        public async Task UpdateUserAsync(string id, UpdateUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new Exception("Usuario no encontrado.");
            }

            // Actualizar campos básicos
            user.Email = dto.Email ?? user.Email;
            user.NombreCompleto = dto.NombreCompleto ?? user.NombreCompleto;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                throw new Exception("Error al actualizar el usuario: " +
                    string.Join(", ", updateResult.Errors.Select(e => e.Description)));
            }

            // Actualizar la contraseña si se proporciona una nueva
            if (!string.IsNullOrEmpty(dto.Password))
            {
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordResult = await _userManager.ResetPasswordAsync(user, resetToken, dto.Password);
                if (!passwordResult.Succeeded)
                {
                    throw new Exception("Error al actualizar la contraseña: " +
                        string.Join(", ", passwordResult.Errors.Select(e => e.Description)));
                }
            }

            // Actualizar el rol
            if (!string.IsNullOrEmpty(dto.Role))
            {
                // Se asume que el usuario solo tiene un rol
                var currentRoles = await _userManager.GetRolesAsync(user);
                if (currentRoles.Any())
                {
                    var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    if (!removeResult.Succeeded)
                    {
                        throw new Exception("Error al eliminar el rol actual: " +
                            string.Join(", ", removeResult.Errors.Select(e => e.Description)));
                    }
                }

                if (await _roleManager.RoleExistsAsync(dto.Role))
                {
                    var addRoleResult = await _userManager.AddToRoleAsync(user, dto.Role);
                    if (!addRoleResult.Succeeded)
                    {
                        throw new Exception("Error al asignar el nuevo rol: " +
                            string.Join(", ", addRoleResult.Errors.Select(e => e.Description)));
                    }
                }
                else
                {
                    throw new Exception($"El rol '{dto.Role}' no existe.");
                }
            }
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new Exception("Usuario no encontrado.");
            }
            return user;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return _userManager.Users.ToList();
        }

        public async Task DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new Exception("Usuario no encontrado.");
            }

            var deleteResult = await _userManager.DeleteAsync(user);
            if (!deleteResult.Succeeded)
            {
                throw new Exception("Error al eliminar el usuario: " +
                    string.Join(", ", deleteResult.Errors.Select(e => e.Description)));
            }
        }
    }
}
