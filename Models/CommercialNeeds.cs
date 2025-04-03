using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EcoinverGMAO_api.Models.Entities;

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

        // Agregar la propiedad que actúa como llave foránea (FK)
        [Required]
        public int IdGenero { get; set; }

        // Nuevo campo solicitado para mostrar el nombre del género
        [Required]
        [MaxLength(200)]
        public string NombreGenero { get; set; }

        public virtual ICollection<CommercialNeedsPlanning> CommercialNeedsPlannings { get; set; }

    }
}
