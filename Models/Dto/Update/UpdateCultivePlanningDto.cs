namespace EcoinverGMAO_api.Models.Dto
{
    public class UpdateCultivePlanningDto
    {
        public string Nombre { get; set; }     // Nombre del planning
        public DateTime? FechaInicio { get; set; }  // Fecha de inicio
        public DateTime? FechaFin { get; set; }     // Fecha de fin
    }
}
