namespace EcoinverGMAO_api.Models.Dto
{
    public class CultiveDataRealDto
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
