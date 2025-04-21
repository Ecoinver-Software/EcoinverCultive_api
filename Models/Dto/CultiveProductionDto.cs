using EcoinverGMAO_api.Models.Entities;

namespace EcoinverGMAO_api.Models.Dto
{
    public class CultiveProductionDto
    {
        public int Id { get; set; }

        // Llave foránea para la relación uno a uno.
        public int CultivePlanningDetailsId { get; set; }

        public string Kilos { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }

        // Propiedad de navegación: relación uno a uno
        //public int CultivePlanningDetails { get; set; }
    }
}