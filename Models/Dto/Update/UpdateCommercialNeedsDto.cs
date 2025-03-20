using System;
using System.ComponentModel.DataAnnotations;

namespace EcoinverGMAO_api.Models.Dto
{
    public class UpdateCommercialNeedsDto
    {
        [MaxLength(100)]
        public string ClientCode { get; set; }

        [MaxLength(200)]
        public string ClientName { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public decimal? Kgs { get; set; }
    }
}
