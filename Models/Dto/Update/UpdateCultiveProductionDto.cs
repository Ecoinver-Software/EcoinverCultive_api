namespace EcoinverGMAO_api.Models.Dto
{
    public class UpdateCultiveProductionDto
    {
        // La planificación/tramo al que pertenece
        public int CultivePlanningDetailsId { get; set; }

        // El cultivo asociado (FK)
        public int CultiveId { get; set; }

        // Kilos “crudos” (pueden venir del detalle o del cliente)
        //public string Kilos { get; set; }

        // Kilos multiplicados por superficie
        //public string KilosAjustados { get; set; }

        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}
