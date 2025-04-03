namespace EcoinverGMAO_api.Models.Dto
{
    public class UpdateCommercialNeedsPlanningDetailsDto
    {
        // Identificador del registro que se va a actualizar
        public int Id { get; set; }

        // Clave foránea
        public int IdCommercialNeedsPlanning { get; set; }

        public double? Kilos { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public int NumeroSemana { get; set; }
    }
}
