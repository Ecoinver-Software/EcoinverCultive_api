using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using EcoinverGMAO_api.Models.Identity;
using EcoinverGMAO_api.Models.Dto;

namespace EcoinverGMAO_api.Services
{
    public interface IRoleService
    {
        Task CreateRoleAsync(CreateRoleDto dto);
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<Role> GetRoleByIdAsync(string id);
        Task UpdateRoleAsync(string id, UpdateRoleDto dto);
        Task DeleteRoleAsync(string id);
    }

    public class RoleService : IRoleService
    {
        private readonly RoleManager<Role> _roleManager;

        public RoleService(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task CreateRoleAsync(CreateRoleDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("El nombre del rol no puede estar vacío.");

            if (await _roleManager.RoleExistsAsync(dto.Name))
                throw new Exception($"El rol '{dto.Name}' ya existe.");

            var role = new Role
            {
                Name = dto.Name,
                Description = dto.Description,
                Level = dto.Level
            };

            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Error al crear el rol: {errors}");
            }
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return _roleManager.Roles.ToList();
        }

        public async Task<Role> GetRoleByIdAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            return role ?? throw new Exception("Rol no encontrado.");
        }

        public async Task UpdateRoleAsync(string id, UpdateRoleDto dto)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                throw new Exception("Rol no encontrado.");

            if (!string.IsNullOrEmpty(dto.Name))
                role.Name = dto.Name;

            if (!string.IsNullOrEmpty(dto.Description))
                role.Description = dto.Description;

            if (dto.Level.HasValue)
                role.Level = dto.Level.Value;

            var result = await _roleManager.UpdateAsync(role);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Error al actualizar el rol: {errors}");
            }
        }

        public async Task DeleteRoleAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                throw new Exception("Rol no encontrado.");

            var result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Error al eliminar el rol: {errors}");
            }
        }
    }
}
