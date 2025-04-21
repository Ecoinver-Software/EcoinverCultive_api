namespace EcoinverGMAO_api.Models.Entities
{
    public class CultiveProduction : BaseModel
    {
        public int Id { get; set; }

        // Llave foránea para la relación uno a uno.
        public int CultivePlanningDetailsId { get; set; }

        public string Kilos { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }

        // Propiedad de navegación: relación uno a uno
        public virtual CultivePlanningDetails CultivePlanningDetails { get; set; }
    }
}
