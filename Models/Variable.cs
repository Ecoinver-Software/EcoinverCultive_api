using System.ComponentModel.DataAnnotations.Schema;
using EcoinverGMAO_api.Models;
using EcoinverGMAO_api.Models.Entities;

namespace EcoinverCultive_api.Models
{
    public class Variable : BaseModel
    {
        public string Name { get; set; }
        public int IdCultivo { get; set; }
        [ForeignKey("IdCultivo")]
        public Cultive Cultivo { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public float Valor { get; set; }
        public string Categoria { get; set; }

    }
}
