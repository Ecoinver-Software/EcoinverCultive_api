namespace EcoinverGMAO_api.Models.Dto
{
    public class UserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string NombreCompleto { get; set; }
        /// <summary>
        /// Rol único que se mostrará para este usuario.
        /// </summary>
        public string Role { get; set; }
    }
}
