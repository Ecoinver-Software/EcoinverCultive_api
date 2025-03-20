using System.ComponentModel.DataAnnotations;

namespace EcoinverGMAO_api.Models.Dto
{
    public class UpdateUserDto
    {
        // En una actualización, el username usualmente no se modifica, por ello se omite.

        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        public string Email { get; set; }

        public string NombreCompleto { get; set; }

        // La contraseña se puede actualizar, si se envía. Si no se envía, se mantiene la actual.
        public string Password { get; set; }

        // Al tener un único rol, se actualiza con un string.
        public string Role { get; set; }
    }
}
