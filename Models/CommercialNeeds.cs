using System;
using System.ComponentModel.DataAnnotations;
using EcoinverGMAO_api.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

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
        // La propiedad que actúa como clave foránea
        [Required]
        public int GeneroId { get; set; }  // Clave foránea que apunta a Gender.IdGenero

        [Required]
        public string GeneroNombre { get; set; }

        [ForeignKey(nameof(GeneroId))]
        public virtual Gender Genero { get; set; }
        [Required]
        public decimal Kgs { get; set; }
    }
}
