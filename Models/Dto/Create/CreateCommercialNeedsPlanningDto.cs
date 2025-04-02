namespace EcoinverGMAO_api.Models.Dto
{
    public class CreateCommercialNeedsPlanningDto
    {
        public int WeekNumber { get; set; }
        public decimal Kgs { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
