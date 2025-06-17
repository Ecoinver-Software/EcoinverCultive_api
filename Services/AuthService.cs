using EcoinverGMAO_api.Models.Dto;
using EcoinverGMAO_api.Models.Identity;
using EcoinverGMAO_api.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EcoinverGMAO_api.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto> AuthenticateAsync(string username, string password);
        Task<LoginResponseDto> AutoLoginAsync(string username, string? email);
    }

    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager; // Inyectamos el RoleManager para roles personalizados
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<User> userManager, RoleManager<Role> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto> AuthenticateAsync(string username, string password)
        {
            // Buscar el usuario por nombre
            var user = await _userManager.FindByNameAsync(username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                throw new NotFoundException("Usuario o contraseña inválidos");
            }

            // Crear claims básicos
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Agregar roles del usuario a los claims (si existen) y añadir el nivel de jerarquía
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var roleName in userRoles)
            {
                // Añadimos el claim del rol
                authClaims.Add(new Claim(ClaimTypes.Role, roleName));

                // Obtenemos la entidad del rol para extraer su nivel jerárquico
                var roleEntity = await _roleManager.FindByNameAsync(roleName);
                if (roleEntity != null)
                {
                    // Se añade el claim "RoleLevel" con el valor del nivel del rol
                    authClaims.Add(new Claim("RoleLevel", roleEntity.Level.ToString()));
                }
            }

            // Leer la configuración JWT
            var jwtSettings = _configuration.GetSection("Jwt");
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));

            // Crear el token
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                expires: DateTime.UtcNow.AddHours(Convert.ToDouble(jwtSettings["DurationInHours"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            // Devolver el token
            return new LoginResponseDto
            {
                UserId = user.Id,
                UserName = user.UserName,
                Token = jwtToken
            };
        }



        //autologin
        public async Task<LoginResponseDto> AutoLoginAsync(string username, string? email)
        {
            // Buscar usuario existente
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                // Crear usuario automáticamente si no existe
                user = new User
                {
                    UserName = username,
                    Email = email ?? $"{username}@intranet.local",
                    EmailConfirmed = true
                };

                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    throw new Exception("Error creando usuario automáticamente");
                }

                // Asignar rol por defecto
                var defaultRole = await _roleManager.FindByNameAsync("User");
                if (defaultRole != null)
                {
                    await _userManager.AddToRoleAsync(user, defaultRole.Name);
                }
            }

            // Usar la misma lógica de tu método AuthenticateAsync para generar el token
            // Crear claims básicos
            var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            // Agregar roles del usuario a los claims (copia exacta de tu método)
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var roleName in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, roleName));
                var roleEntity = await _roleManager.FindByNameAsync(roleName);
                if (roleEntity != null)
                {
                    authClaims.Add(new Claim("RoleLevel", roleEntity.Level.ToString()));
                }
            }

            // Leer la configuración JWT (copia exacta de tu método)
            var jwtSettings = _configuration.GetSection("Jwt");
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));

            // Crear el token (copia exacta de tu método)
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                expires: DateTime.UtcNow.AddHours(Convert.ToDouble(jwtSettings["DurationInHours"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            // Devolver el mismo formato que tu método AuthenticateAsync
            return new LoginResponseDto
            {
                UserId = user.Id,
                UserName = user.UserName,
                Token = jwtToken
            };
        }
    }
}
