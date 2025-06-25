using System;
using System.ComponentModel.DataAnnotations;

namespace EcoinverGMAO_api.Models.Dto
{
    public class CreateCommercialNeedsDto
    {
        [Required(ErrorMessage = "El código del cliente es obligatorio.")]
        public int ClientCode { get; set; }

        [Required(ErrorMessage = "El nombre del cliente es obligatorio.")]
        [MaxLength(200)]
        public string ClientName { get; set; }
         
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "La cantidad en kgs es obligatoria.")]
        public decimal Kgs { get; set; }

        // Campos para el género
        [Required(ErrorMessage = "El Id del género es obligatorio.")]
        public int IdGenero { get; set; }

        [Required(ErrorMessage = "El nombre del género es obligatorio.")]
        [MaxLength(200)]
        public string NombreGenero { get; set; }
    }
}
