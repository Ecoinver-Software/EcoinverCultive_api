using System.ComponentModel.DataAnnotations.Schema;

namespace EcoinverGMAO_api.Models
{
    
     public class ControlStock : BaseModel
    {
        public DateTime Fecha { get; set; }
        

        // Propiedad de navegación para la relación uno a muchos
        public virtual ICollection<ControlStockDetails> ControlStockDetails { get; set; }
    }
}
