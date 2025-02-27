using System;
using System.ComponentModel.DataAnnotations;

namespace EcoinverGMAO_api.Models
{
    public class Company : BaseModel
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(300)]
        public string? Address { get; set; }

        [MaxLength(50)]
        public string? Phone { get; set; }

        [MaxLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [MaxLength(20)]
        public string CIF { get; set; }
    }
}
