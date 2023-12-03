using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EdgeCut.Models
{
    public class Furniture : BaseEntity
    {
        [StringLength(100)]
        public string Name { get; set; }
        public double Price { get; set; }

        [StringLength(100)]
        public string? Image {  get; set; }

        [NotMapped]
        public IFormFile File {  get; set; }
    }
}
