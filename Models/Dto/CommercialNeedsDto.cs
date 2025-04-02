using System;

namespace EcoinverGMAO_api.Models.Dto
{
    public class CommercialNeedsDto
    {
        public int Id { get; set; }
        public int ClientCode { get; set; }
        public string ClientName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal Kgs { get; set; }

        public int GeneroId { get; set; }
        public string GeneroName { get; set; }
    }
}
