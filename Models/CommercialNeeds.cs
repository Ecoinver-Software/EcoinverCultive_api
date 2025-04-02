using System;
using System.ComponentModel.DataAnnotations;

namespace EcoinverGMAO_api.Models
{
    public class CommercialNeeds : BaseModel
    {
        [Required]
        public int ClientCode { get; set; }

        [Required]
        [MaxLength(200)]
        public string ClientName { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        public decimal Kgs { get; set; }
    }
}
