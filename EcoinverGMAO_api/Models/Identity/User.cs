using Microsoft.AspNetCore.Identity;

namespace EcoinverGMAO_api.Models.Identity
{
    public class User : IdentityUser
    {
        // Propiedad extra de ejemplo
        public string NombreCompleto { get; set; }

    }
}
