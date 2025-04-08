using System.ComponentModel.DataAnnotations.Schema;

namespace EcoinverGMAO_api.Models.Entities
{
    public class CultivePlanningDetails : BaseModel
    {
        public int Id { get; set; }
        public DateTime? FechaInicio { get; set; }    // Fecha de inicio
        public DateTime? FechaFin { get; set; }       // Fecha de fin

        public double? Kilos { get; set; }            // Cantidad de kilos
        public int Tramo { get; set; }                // Tramo (1 a 12)

        // Clave foránea que enlaza con CultivePlanning
        [ForeignKey("CultivePlanning")]
        public int CultivePlanningId { get; set; }

        // Propiedad de navegación
        public virtual CultivePlanning CultivePlanning { get; set; }
    }
}
