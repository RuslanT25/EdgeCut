using EdgeCut.DAL;
using EdgeCut.Models;
using EdgeCut.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EdgeCut.Controllers
{
    public class HomeController : Controller
    {
        readonly ApplicationContext _context;
        public HomeController(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {   
            ViewBag.Page = "Home";
            List<Slider> sliders = await _context.Sliders.Where(x => x.DeletedAt == null).ToListAsync();
            List<Blog> blogs = await _context.Blogs.Where(x => x.DeletedAt == null).ToListAsync();
            List<Furniture> furnitures = await _context.Furnitures.Where(x => x.DeletedAt == null).ToListAsync();
            List<Testimonial> testimonials = await _context.Testimonials.Where(x => x.DeletedAt == null).ToListAsync();

            HomeViewModel homeViewModel = new()
            {
                Sliders = sliders,
                Blogs = blogs,
                Furnitures = furnitures,
                Testimonials = testimonials
            };

            return View(homeViewModel);
        }
    }
}
