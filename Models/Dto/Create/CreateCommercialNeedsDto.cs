using System;
using System.ComponentModel.DataAnnotations;

namespace EcoinverGMAO_api.Models.Dto
{
    public class CreateCommercialNeedsDto
    {
        [Required(ErrorMessage = "El código del cliente es obligatorio.")]
        [MaxLength(100)]
        public int ClientCode { get; set; }

        [Required(ErrorMessage = "El nombre del cliente es obligatorio.")]
        [MaxLength(200)]
        public string ClientName { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "La cantidad en kgs es obligatoria.")]
        public decimal Kgs { get; set; }
    }
}
