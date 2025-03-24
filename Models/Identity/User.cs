using Microsoft.AspNetCore.Identity;

namespace EcoinverGMAO_api.Models.Identity
{
    public class User : IdentityUser
    {
        public string NombreCompleto { get; set; }

        // Ejemplo de cómo ignorar el hash si llegases a retornar "User" sin mapearlo
        // [JsonIgnore]
        // public override string PasswordHash { get; set; }
    }
}
