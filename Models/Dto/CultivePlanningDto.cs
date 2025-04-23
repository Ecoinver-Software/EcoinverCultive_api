namespace EcoinverGMAO_api.Models.Dto
{
    public class CultivePlanningDto
    {
        public int Id { get; set; } //id
        public string Nombre { get; set; }     // Nombre del planning
        public DateTime? FechaInicio { get; set; }  // Fecha de inicio
        public DateTime? FechaFin { get; set; }     // Fecha de fin
        public int? IdGenero { get; set; }
    }
}
