using System.ComponentModel.DataAnnotations;

namespace EcoinverGMAO_api.Models
{
    public class JobPosition : BaseModel
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(100)]
        public string Department { get; set; }
    }
}
