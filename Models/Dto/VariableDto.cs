namespace EcoinverCultive_api.Models.Dto
{
    public class VariableDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int IdCultivo {  get; set; }
        public DateTime FechaRegistro { get; set; }
        public float Valor {  get; set; }
    }
}
