using System.ComponentModel.DataAnnotations.Schema;

namespace EcoinverGMAO_api.Models.Entities
{
    public class CultivePlanning : BaseModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }     // Nombre del planning
        public DateTime? FechaInicio { get; set; }  // Fecha de inicio
        public DateTime? FechaFin { get; set; }     // Fecha de fin

        // Relación de uno a muchos con CultivePlanningDetails
        public virtual ICollection<CultivePlanningDetails> CultivePlanningDetails { get; set; }

        // 1) Añade la FK y la navegación a Gender
        public int? IdGenero { get; set; }

        [ForeignKey(nameof(IdGenero))]
        public virtual Gender Genero { get; set; }
    }
}
