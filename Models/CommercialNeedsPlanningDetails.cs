namespace EcoinverGMAO_api.Models.Entities
{
    public class CommercialNeedsPlanningDetails : BaseModel
    {
        public int IdCommercialNeedsPlanning { get; set; }  // Clave foránea

        public double? Kilos { get; set; }                  // Kilos necesarios
        public DateTime? FechaDesde { get; set; }           // Fecha de inicio
        public DateTime? FechaHasta { get; set; }           // Fecha de fin
        public int NumeroSemana { get; set; }               // Número de semana

        // Propiedad de navegación (opcional pero recomendable si usas EF)
        public virtual CommercialNeedsPlanning CommercialNeedsPlanning { get; set; }
    }
}
