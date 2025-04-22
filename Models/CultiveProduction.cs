using System.ComponentModel.DataAnnotations.Schema;

namespace EcoinverGMAO_api.Models.Entities
{
    public class CultiveProduction : BaseModel
    {
        public int Id { get; set; }

        // Llave foránea a los detalles de planificación
        public int CultivePlanningDetailsId { get; set; }
        public virtual CultivePlanningDetails CultivePlanningDetails { get; set; }

        public string Kilos { get; set; }
        public string KilosAjustados { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }

        // FK a Cultive
        public int CultiveId { get; set; }
        [ForeignKey(nameof(CultiveId))]
        public virtual Cultive Cultive { get; set; }
    }
}
