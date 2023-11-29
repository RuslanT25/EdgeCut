using EdgeCut.DAL;
using EdgeCut.Models;
using EdgeCut.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace EdgeCut.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        readonly ApplicationContext _context;
        readonly IFileService _fileService;
        public SliderController(ApplicationContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        // GET: SliderController
        public ActionResult Index()
        {
            List<Slider> sliders = _context.Sliders.ToList();

            return View(sliders);
        }

        // GET: SliderController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            Slider slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (slider is null) return NotFound("Bu sekil tapilmadi");

            return View(slider);
        }

        // GET: SliderController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SliderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(slider);
                }

                (int status, string message) = await _fileService.FileUpload("sliders", slider.File);
                if (status == 0)
                {
                    ModelState.AddModelError("File", message);
                }

                if (!ModelState.IsValid)
                {
                    return View(slider);
                }

                slider.Image = message;

                _context.Sliders.Add(slider);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return Content(ex.Message);
            }
        }

        // GET: SliderController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SliderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SliderController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SliderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
