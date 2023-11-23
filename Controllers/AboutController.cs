using Microsoft.AspNetCore.Mvc;

namespace EdgeCut.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Page = "About";
            return View();
        }
    }
}
