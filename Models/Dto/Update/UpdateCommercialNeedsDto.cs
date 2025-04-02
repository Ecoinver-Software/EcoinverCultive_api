using System;
using System.ComponentModel.DataAnnotations;

namespace EcoinverGMAO_api.Models.Dto
{
    public class UpdateCommercialNeedsDto
    {
        public int ClientCode { get; set; }

        [MaxLength(200)]
        public string ClientName { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public decimal? Kgs { get; set; }

        
        public int GeneroId { get; set; }

       
        public string GeneroNombre { get; set; }
    }
}
