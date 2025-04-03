using System.ComponentModel.DataAnnotations.Schema;

namespace EcoinverGMAO_api.Models.Entities
{
    public class CommercialNeedsPlanning : BaseModel
    {
        [ForeignKey("CommercialNeed")]
        public int IdCommercialNeed { get; set; }
        public int WeekNumber { get; set; }
        public decimal Kgs { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public virtual CommercialNeeds CommercialNeed { get; set; }
    }
}
