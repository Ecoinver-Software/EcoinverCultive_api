namespace EcoinverGMAO_api.Models.Entities
{
    public class Cultive : BaseModel
    {
        public int Id { get; set; } // Clave primaria autoincremental local

        public int IdCultivo { get; set; } // ID original del ERP
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

        // Nuevos campos para latitud y longitud (almacenados como varchar(9))
        public string Latitud { get; set; }
        public string Longitud { get; set; }

        // Nuevo campo FK para el planning de cultivo
        public int? IdCultivePlanning { get; set; }

        // Propiedad de navegación para la tabla CultivePlanning
        // (opcional pero recomendable si utilizas Entity Framework)
        public virtual CultivePlanning CultivePlanning { get; set; }
    }
}
