using EdgeCut.DAL;
using EdgeCut.Models;
using EdgeCut.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using NuGet.Protocol;

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
        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _context.Sliders.Where(x => x.DeletedAt == null).ToListAsync();

            return View(sliders);
        }

        // GET: SliderController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            Slider slider = await _context.Sliders.Where(x => x.DeletedAt == null).FirstOrDefaultAsync(s => s.Id == id);
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
                TempData["Message"] = "Slider has ben created successfully";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        // GET: SliderController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            Slider slider = await _context.Sliders.Where(x => x.DeletedAt == null).FirstOrDefaultAsync(s => s.Id == id);
            if (slider == null) return NotFound();

            return View(slider);
        }

        // POST: SliderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Slider model)
        {
            try
            {
                Slider slider = await _context.Sliders.Where(x => x.DeletedAt == null).FirstOrDefaultAsync(s => s.Id == id);
                if (slider == null) return NotFound();
                if (model.File != null)
                {
                    (int status, string message) = await _fileService.FileUpload("sliders", model.File);
                    if (status == 0)
                    {
                        ModelState.AddModelError("File", message);
                        return View(slider);
                    }

                    _fileService.DeleteFile("sliders", slider.Image);
                    slider.Image = message;
                }

                slider.Title = model.Title;
                slider.Description = model.Description;
                slider.UpdatedAt = DateAndTime.Now;
                await _context.SaveChangesAsync();
                TempData["Message"] = "Slider has ben updated successfully";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        // POST: SliderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                Slider slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
                if (slider == null) return NotFound();

                slider.DeletedAt = DateAndTime.Now;
                _fileService.DeleteFile("sliders", slider.Image);
                await _context.SaveChangesAsync();

                return Json(new
                {
                    Status = true,
                    Message = "Slider has been deleted"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Status = false,
                    Message = "Something went wrong"
                });
            }
        }
    }
}
