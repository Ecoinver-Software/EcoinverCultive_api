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

        // Campos para el género (se usan tipos anulables si quieres que sean opcionales)
        public int? IdGenero { get; set; }

        [MaxLength(200)]
        public string NombreGenero { get; set; }
    }
}
