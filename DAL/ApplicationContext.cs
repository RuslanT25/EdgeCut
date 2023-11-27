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
    }
}
