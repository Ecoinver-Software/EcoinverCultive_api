using EcoinverGMAO_api.Models.Entities;

namespace EcoinverGMAO_api.Models.Dto
{
    public class CultivePlanningDetailsDto
    {
        public int Id { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public double? Kilos { get; set; }
        public int Tramo { get; set; }
        public int CultivePlanningId { get; set; }  // Asegúrate que esta propiedad se llama CultivePlanningId
    }
}