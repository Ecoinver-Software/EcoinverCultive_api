namespace EcoinverCultive_api.Models.Dto.Create
{
    public class CreateVariableDto
    {
        public string Name { get; set; }
        public int IdCultivo { get; set; }
        public DateTime FechaRegistro { get; set; }
        public float Valor { get; set; }
        
    }
}
