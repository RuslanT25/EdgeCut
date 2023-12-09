using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EdgeCut.Models
{
    public class Testimonial : BaseEntity
    { 
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(2000)]
        public string? Description { get; set; }

        [StringLength(100)]
        public string? Image { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }
    }
}
