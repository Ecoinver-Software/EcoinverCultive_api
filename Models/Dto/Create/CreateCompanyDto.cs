using System.ComponentModel.DataAnnotations;

namespace EcoinverGMAO_api.Models.Dto
{
    public class CreateCompanyDto
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        public string NombreCompleto { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio.")]
        public string Role { get; set; }
    }
}
