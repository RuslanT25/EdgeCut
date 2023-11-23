using Microsoft.AspNetCore.Mvc;

namespace EdgeCut.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {   
            ViewBag.Page = "Home";
            return View();
        }
    }
}
