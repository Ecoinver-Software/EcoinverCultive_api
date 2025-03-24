namespace EcoinverGMAO_api.Models.Dto
{
    public class CreateUserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string NombreCompleto { get; set; }
        public string Password { get; set; }
        /// <summary>
        /// Rol único que tendrá este usuario.
        /// </summary>
        public string Role { get; set; }
    }
}
