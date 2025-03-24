namespace EcoinverGMAO_api.Models.Dto
{
    public class CreateUserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string NombreCompleto { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}