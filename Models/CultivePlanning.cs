namespace EcoinverGMAO_api.Models.Entities
{
    public class CultivePlanning : BaseModel
    {
        public string Nombre { get; set; }     // Nombre del planning
        public DateTime? FechaInicio { get; set; }  // Fecha de inicio
        public DateTime? FechaFin { get; set; }     // Fecha de fin
    }
}
