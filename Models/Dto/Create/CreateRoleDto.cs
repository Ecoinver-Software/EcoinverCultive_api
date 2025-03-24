namespace EcoinverGMAO_api.Models.Dto
{
    public class CreateRoleDto
    {
        public string Name { get; set; } // Permite actualizar el nombre si se desea
        public string Description { get; set; }
        public int Level { get; set; }
    }
}
