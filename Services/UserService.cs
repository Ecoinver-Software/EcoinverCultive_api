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
        Task<User> CreateUserAsync(CreateUserDto dto);
        Task<User> UpdateUserAsync(string id, UpdateUserDto dto);
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

        public async Task<User> CreateUserAsync(CreateUserDto dto)
        {
            // Verificar si el usuario ya existe (por username)
            var existingUser = await _userManager.FindByNameAsync(dto.UserName);
            if (existingUser != null)
            {
                throw new Exception("El usuario ya existe.");
            }

            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                NombreCompleto = dto.NombreCompleto,
                EmailConfirmed = true
            };

            // Crear el usuario con la contraseña indicada
            var createResult = await _userManager.CreateAsync(user, dto.Password);
            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                throw new Exception($"Error al crear el usuario: {errors}");
            }

            // Asignar el rol único si se especifica
            if (!string.IsNullOrEmpty(dto.Role))
            {
                if (await _roleManager.RoleExistsAsync(dto.Role))
                {
                    var addRoleResult = await _userManager.AddToRoleAsync(user, dto.Role);
                    if (!addRoleResult.Succeeded)
                    {
                        var errors = string.Join(", ", addRoleResult.Errors.Select(e => e.Description));
                        throw new Exception($"Error al asignar el rol: {errors}");
                    }
                }
                else
                {
                    throw new Exception($"El rol '{dto.Role}' no existe.");
                }
            }

            return user;
        }

        public async Task<User> UpdateUserAsync(string id, UpdateUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new Exception("Usuario no encontrado.");
            }

            // Actualizar campos básicos
            if (!string.IsNullOrEmpty(dto.Email))
                user.Email = dto.Email;

            if (!string.IsNullOrEmpty(dto.NombreCompleto))
                user.NombreCompleto = dto.NombreCompleto;
            // Actualizar el UserName si se proporciona
            if (!string.IsNullOrEmpty(dto.UserName))
                user.UserName = dto.UserName;
            // Guardar los cambios en el usuario
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
                throw new Exception($"Error al actualizar el usuario: {errors}");
            }

            // Actualizar la contraseña si se proporciona
            if (!string.IsNullOrEmpty(dto.Password))
            {
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordResult = await _userManager.ResetPasswordAsync(user, resetToken, dto.Password);
                if (!passwordResult.Succeeded)
                {
                    var errors = string.Join(", ", passwordResult.Errors.Select(e => e.Description));
                    throw new Exception($"Error al actualizar la contraseña: {errors}");
                }
            }

            // Actualizar el rol (asumiendo un único rol por usuario)
            if (!string.IsNullOrEmpty(dto.Role))
            {
                // Eliminar el rol actual si existe
                var currentRoles = await _userManager.GetRolesAsync(user);
                if (currentRoles.Any())
                {
                    var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    if (!removeResult.Succeeded)
                    {
                        var errors = string.Join(", ", removeResult.Errors.Select(e => e.Description));
                        throw new Exception($"Error al eliminar roles actuales: {errors}");
                    }
                }

                // Asignar el nuevo rol
                if (await _roleManager.RoleExistsAsync(dto.Role))
                {
                    var addRoleResult = await _userManager.AddToRoleAsync(user, dto.Role);
                    if (!addRoleResult.Succeeded)
                    {
                        var errors = string.Join(", ", addRoleResult.Errors.Select(e => e.Description));
                        throw new Exception($"Error al asignar el nuevo rol: {errors}");
                    }
                }
                else
                {
                    throw new Exception($"El rol '{dto.Role}' no existe.");
                }
            }

            return user;
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
            // .Users es un IQueryable<User>, conviene usar ToListAsync en vez de ToList() si quieres async real.
            // Pero ToList() es suficiente si no son muchos registros.
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
                var errors = string.Join(", ", deleteResult.Errors.Select(e => e.Description));
                throw new Exception($"Error al eliminar el usuario: {errors}");
            }
        }
    }
}
