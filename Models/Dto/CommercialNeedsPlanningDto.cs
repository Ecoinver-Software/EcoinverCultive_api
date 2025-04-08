namespace EcoinverGMAO_api.Models.Dto
{
    public class CommercialNeedsPlanningDto
    {
        public int Id { get; set; }
        public int IdCommercialNeed { get; set; }
        public int WeekNumber { get; set; }
        public decimal Kgs { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
