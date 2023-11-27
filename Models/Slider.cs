using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EdgeCut.Models
{
    public class Slider : BaseEntity
    {
        [StringLength(100), Required(ErrorMessage = "Basliq teleb olunur")]
        public string Title { get; set; }
        [StringLength(800)]
        public string Description { get; set; }
        [StringLength(100)]
        public string? Image { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }
    }
}
