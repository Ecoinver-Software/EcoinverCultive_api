namespace EcoinverGMAO_api.Models.Entities
{
    public class Gender : BaseModel
    {
        public int IdGenero { get; set; }
        public string NombreGenero { get; set; }
        public string IdFamilia { get; set; }
        public string NombreFamilia { get; set; }


        // Navegación inversa 1:1
        public virtual CultivePlanning CultivePlanning { get; set; }
    }

}
