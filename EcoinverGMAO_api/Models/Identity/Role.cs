using Microsoft.AspNetCore.Identity;

namespace EcoinverGMAO_api.Models.Identity
{
    public class Role : IdentityRole
    {
        public string Description { get; set; }
        public int Level { get; set; } // Por ejemplo, para definir jerarquía (1: más alto, 99: básico)
    }
}
