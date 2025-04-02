using EcoinverGMAO_api.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace EcoinverGMAO_api.Seeders
{
    public class DataSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            // Crear un scope para obtener los servicios necesarios
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

            // Definir nombres de roles y usuarios
            string developerRoleName = "Desarrollador";
            string adminRoleName = "Admin";
            string technicalRoleName = "Tecnico de campo";
            string comercialRoleName = "Comercial";

            string adminUserName = "Admin";
            string tecnicoUserName = "Tecnico";
            string comercialUserName = "Comercial";

            // Crear el rol de desarrollador si no existe
            if (!await roleManager.RoleExistsAsync(developerRoleName))
            {
                var role = new Role
                {
                    Name = developerRoleName,
                    Description = "Rol de desarrollador con permisos completos para desarrolladores.",
                    Level = 1
                };

                var roleResult = await roleManager.CreateAsync(role);
                if (!roleResult.Succeeded)
                {
                    throw new Exception("Error al crear el rol de desarrollador: " +
                        string.Join(", ", roleResult.Errors));
                }
            }

            // Crear el rol de admin si no existe
            if (!await roleManager.RoleExistsAsync(adminRoleName))
            {
                var role = new Role
                {
                    Name = adminRoleName,
                    Description = "Rol de administrador con permisos completos.",
                    Level = 2
                };

                var roleResult = await roleManager.CreateAsync(role);
                if (!roleResult.Succeeded)
                {
                    throw new Exception("Error al crear el rol de admin: " +
                        string.Join(", ", roleResult.Errors));
                }
            }

            // Crear el rol de tecnico de campo si no existe
            if (!await roleManager.RoleExistsAsync(technicalRoleName))
            {
                var role = new Role
                {
                    Name = technicalRoleName,
                    Description = "Rol de técnico de campo con permisos para tareas de campo.",
                    Level = 3
                };

                var roleResult = await roleManager.CreateAsync(role);
                if (!roleResult.Succeeded)
                {
                    throw new Exception("Error al crear el rol de tecnico de campo: " +
                        string.Join(", ", roleResult.Errors));
                }
            }

            // Crear el rol de comercial si no existe
            if (!await roleManager.RoleExistsAsync(comercialRoleName))
            {
                var role = new Role
                {
                    Name = comercialRoleName,
                    Description = "Rol de comercial con permisos limitados para tareas comerciales.",
                    Level = 4
                };

                var roleResult = await roleManager.CreateAsync(role);
                if (!roleResult.Succeeded)
                {
                    throw new Exception("Error al crear el rol de comercial: " +
                        string.Join(", ", roleResult.Errors));
                }
            }

            // Crear el usuario admin si no existe
            var adminUser = await userManager.FindByNameAsync(adminUserName);
            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = adminUserName,
                    Email = "admin@ecoinver.es",
                    NombreCompleto = "Administrador del Sistema",
                    EmailConfirmed = true
                };

                var userResult = await userManager.CreateAsync(adminUser, "1234");
                if (!userResult.Succeeded)
                {
                    throw new Exception("Error al crear el usuario admin: " +
                        string.Join(", ", userResult.Errors));
                }

                var addToRoleResult = await userManager.AddToRoleAsync(adminUser, adminRoleName);
                if (!addToRoleResult.Succeeded)
                {
                    throw new Exception("Error al asignar el rol de admin: " +
                        string.Join(", ", addToRoleResult.Errors));
                }
            }

            // Crear el usuario tecnico si no existe
            var tecnicoUser = await userManager.FindByNameAsync(tecnicoUserName);
            if (tecnicoUser == null)
            {
                tecnicoUser = new User
                {
                    UserName = tecnicoUserName,
                    Email = "tecnico@ecoinver.es",
                    NombreCompleto = "Técnico de Campo",
                    EmailConfirmed = true
                };

                var userResult = await userManager.CreateAsync(tecnicoUser, "1234");
                if (!userResult.Succeeded)
                {
                    throw new Exception("Error al crear el usuario técnico: " +
                        string.Join(", ", userResult.Errors));
                }

                var addToRoleResult = await userManager.AddToRoleAsync(tecnicoUser, technicalRoleName);
                if (!addToRoleResult.Succeeded)
                {
                    throw new Exception("Error al asignar el rol de tecnico de campo: " +
                        string.Join(", ", addToRoleResult.Errors));
                }
            }

            // Crear el usuario comercial si no existe
            var comercialUser = await userManager.FindByNameAsync(comercialUserName);
            if (comercialUser == null)
            {
                comercialUser = new User
                {
                    UserName = comercialUserName,
                    Email = "comercial@ecoinver.es",
                    NombreCompleto = "Usuario Comercial",
                    EmailConfirmed = true
                };

                var userResult = await userManager.CreateAsync(comercialUser, "1234");
                if (!userResult.Succeeded)
                {
                    throw new Exception("Error al crear el usuario comercial: " +
                        string.Join(", ", userResult.Errors));
                }

                var addToRoleResult = await userManager.AddToRoleAsync(comercialUser, comercialRoleName);
                if (!addToRoleResult.Succeeded)
                {
                    throw new Exception("Error al asignar el rol de comercial: " +
                        string.Join(", ", addToRoleResult.Errors));
                }
            }
        }
    }
}
