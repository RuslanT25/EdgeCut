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
    public class TestimonialController : Controller
    {
        readonly ApplicationContext _context;
        readonly IFileService _fileService;
        public TestimonialController(ApplicationContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        // GET: TestimonialController
        public async Task<IActionResult> Index()
        {
            List<Testimonial> testimonials = await _context.Testimonials.Where(x=>x.DeletedAt == null).ToListAsync();

            return View(testimonials);
        }

        // GET: TestimonialController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            Testimonial testimonial = await _context.Testimonials.Where(x => x.DeletedAt == null).FirstOrDefaultAsync(s => s.Id == id);
            if (testimonial is null) return NotFound("Bu sekil tapilmadi");

            return View(testimonial);
        }

        // GET: TestimonialController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BlogController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Testimonial testimonial)
        {
            try
            {
                if (testimonial.File == null)
                {
                    ModelState.AddModelError("File", "File is required");
                }

                (int status, string message) = await _fileService.FileUpload("testimonials", testimonial.File);
                if (status == 0)
                {
                    ModelState.AddModelError("File", message);
                }

                if (!ModelState.IsValid)
                {
                    return View(testimonial);
                }

                testimonial.Image = message;

                await _context.Testimonials.AddAsync(testimonial);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Testimonial has ben created successfully";

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BlogController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            Testimonial testimonial = await _context.Testimonials.Where(x => x.DeletedAt == null).FirstOrDefaultAsync(s => s.Id == id);
            if (testimonial is null) return NotFound("Bu sekil tapilmadi");

            return View(testimonial);
        }

        // POST: BlogController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Testimonial model)
        {
            try
            {
                Testimonial testimonial = await _context.Testimonials.Where(x => x.DeletedAt == null).FirstOrDefaultAsync(f => f.Id == id);
                if (testimonial == null) return NotFound();
                if (model.File != null)
                {
                    (int status, string message) = await _fileService.FileUpload("testimonials", model.File);
                    if (status == 0)
                    {
                        ModelState.AddModelError("File", message);
                        return View(testimonial);
                    }

                    _fileService.DeleteFile("testimonials", testimonial.Image);
                    testimonial.Image = message;
                }

                testimonial.Name = model.Name;
                testimonial.Description = model.Description;
                testimonial.UpdatedAt = DateAndTime.Now;
                await _context.SaveChangesAsync();
                TempData["Message"] = "Testimonial has ben updated successfully";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        // GET: BlogController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                Testimonial testimonial = await _context.Testimonials.FirstOrDefaultAsync(f => f.Id == id);
                if (testimonial == null) return NotFound();

                testimonial.DeletedAt = DateAndTime.Now;
                _fileService.DeleteFile("testimonials", testimonial.Image);
                await _context.SaveChangesAsync();

                return Json(new
                {
                    Status = true,
                    Message = "Testimonial has been deleted"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Status = false,
                    Message = ex.Message
                });
            }
        }
    }
}
