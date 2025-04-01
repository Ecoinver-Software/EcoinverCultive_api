namespace EcoinverGMAO_api.Models.Dto
{
    public class CultiveSyncDto
    {
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
        // Nuevos campos
        public string Latitud { get; set; }
        public string Longitud { get; set; }
    }

}
