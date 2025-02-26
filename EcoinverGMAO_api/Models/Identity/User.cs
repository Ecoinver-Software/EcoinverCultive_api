using Microsoft.AspNetCore.Identity;
using EcoinverGMAO_api.Models;

namespace EcoinverGMAO_api.Models.Identity
{
    public class User : IdentityUser
    {
        public string NombreCompleto { get; set; }

        public int? JobPositionId { get; set; }

        public JobPosition JobPosition { get; set; }
    }
}
