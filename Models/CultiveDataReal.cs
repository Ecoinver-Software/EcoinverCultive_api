using System.ComponentModel.DataAnnotations.Schema;

namespace EcoinverGMAO_api.Models.Entities
{
    public class CultiveDataReal : BaseModel
    {
        public int Id { get; set; }
        public int IdCultivo { get; set; }
        public int? IdAgricultor { get; set; }
        public string NombreAgricultor { get; set; }
        public int? IdFinca { get; set; }
        public int? IdGenero { get; set; }
        public double? Superficie { get; set; }
        public double? KilosNetos { get; set; }
        public double? KilosM2 { get; set; }

    }
}
