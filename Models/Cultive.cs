using System.ComponentModel.DataAnnotations.Schema;

namespace EcoinverGMAO_api.Models.Entities
{
    public class Cultive : BaseModel
    {
        public int Id { get; set; } 

        public int IdCultivo { get; set; }

        public int? IdAgricultor { get; set; }
        public string NombreAgricultor { get; set; }

        public int? IdFinca { get; set; }
        public string NombreFinca { get; set; }

        public int? IdNave { get; set; }
        public string NombreNave { get; set; }

        public int? IdGenero { get; set; }
        public string NombreGenero { get; set; }

        public string NombreVariedad { get; set; }

        public double? Superficie { get; set; }
        public double? ProduccionEstimada { get; set; }

        public DateTime? FechaSiembra { get; set; }
        public DateTime? FechaFin { get; set; }

        // Campos de latitud y longitud como varchar(9) en base de datos
        public string Latitud { get; set; }
        public string Longitud { get; set; }

        // FK para la tabla CultivePlanning
        public int? IdCultivePlanning { get; set; }

        [ForeignKey(nameof(IdCultivePlanning))]
        public virtual CultivePlanning CultivePlanning { get; set; }
    }
}
