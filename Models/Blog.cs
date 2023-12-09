using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EdgeCut.Models
{
    public class Blog : BaseEntity
    {
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(2000)]
        public string? Description { get; set; }

        [StringLength(100)]
        public string? Image { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }
    }
}
