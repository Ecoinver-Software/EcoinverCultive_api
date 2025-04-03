using System.ComponentModel.DataAnnotations.Schema;

namespace EcoinverGMAO_api.Models.Entities
{
    public class CommercialNeedsPlanningDetails : BaseModel
    {
        public int IdCommercialNeedsPlanning { get; set; }  // Clave foránea con tu nombre preferido

        public double? Kilos { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public int NumeroSemana { get; set; }

        [ForeignKey("IdCommercialNeedsPlanning")]
        public virtual CommercialNeedsPlanning CommercialNeedsPlanning { get; set; }
    }

}
