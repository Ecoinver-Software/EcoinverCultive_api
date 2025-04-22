using EcoinverGMAO_api.Models.Entities;

namespace EcoinverGMAO_api.Models.Dto
{
    public class CreateCultiveProductionDto
    {
        public int CultivePlanningDetailsId { get; set; }
        public int CultiveId { get; set; }
        public string Kilos { get; set; }
        
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}