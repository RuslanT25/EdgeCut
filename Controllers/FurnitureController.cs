using Microsoft.AspNetCore.Mvc;

namespace EdgeCut.Controllers
{
    public class FurnitureController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Page = "Furniture";
            return View();
        }
    }
}
