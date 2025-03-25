namespace EcoinverGMAO_api.Models.Entities
{
    public class Client : BaseModel
    {
        public int ClientId { get; set; }  // Clave primaria
        public string Name { get; set; }
    }
}
