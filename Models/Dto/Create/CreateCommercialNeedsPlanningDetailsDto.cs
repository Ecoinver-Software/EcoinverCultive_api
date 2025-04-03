namespace EcoinverGMAO_api.Models.Dto
{
    public class CreateCommercialNeedsPlanningDetailsDto
    {
        // Clave foránea
        public int IdCommercialNeedsPlanning { get; set; }

        public double? Kilos { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public int NumeroSemana { get; set; }
    }
}
