using System.ComponentModel.DataAnnotations.Schema;
using EcoinverGMAO_api.Models.Entities;

namespace EcoinverGMAO_api.Models
{
    public class ControlStockDetails : BaseModel
    {
        public int numBultos { get; set; }
        public int CodigoPartida { get; set; }
        public int? IdGenero { get; set; }
        public string? Categoria { get; set; }

        // Clave foránea hacia ControlStock
        public int IdControl { get; set; }

        // Propiedad de navegación
        [ForeignKey("IdControl")]
        public virtual ControlStock ControlStock { get; set; }

       
    }
}
