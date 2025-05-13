namespace EcoinverGMAO_api.Models.Entities
{
    public class Gender : BaseModel
    {
        public int IdGenero { get; set; }
        public string NombreGenero { get; set; }
        public string IdFamilia { get; set; }
        public string NombreFamilia { get; set; }


        // Navegación inversa 1:N
        public virtual ICollection<CultivePlanning> CultivePlannings { get; set; }
            = new List<CultivePlanning>();

        // Propiedad de navegación para la relación uno a muchos
        public virtual ICollection<ControlStockDetails> ControlStockDetails { get; set; }
    }

}
