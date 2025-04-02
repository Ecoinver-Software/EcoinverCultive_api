namespace EcoinverGMAO_api.Models.Entities
{
    public class Gender : BaseModel
    {
        public int IdGenero { get; set; }
        public string NombreGenero { get; set; }
        public string IdFamilia { get; set; }
        public string NombreFamilia { get; set; }
        public virtual ICollection<CommercialNeeds> CommercialNeeds { get; set; } = new List<CommercialNeeds>();
    }

}
