using EdgeCut.Areas.Admin.Controllers;
using EdgeCut.Models;
using Microsoft.EntityFrameworkCore;

namespace EdgeCut.DAL
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Furniture> Furnitures { get; set;}
        public DbSet<Blog> Blogs { get; set;}
        public DbSet<Testimonial> Testimonials { get; set;}
    }
}
