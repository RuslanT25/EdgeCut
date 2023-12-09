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
    public class FurnitureController : Controller
    {
        readonly ApplicationContext _context;
        readonly IFileService _fileService;
        public FurnitureController(ApplicationContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }
        public async Task<IActionResult> Index()
        {
            List<Furniture> furnitures = await _context.Furnitures.Where(x => x.DeletedAt == null).ToListAsync();

            return View(furnitures);
        }

        // GET: FurnitureController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            Furniture furniture = await _context.Furnitures.Where(x => x.DeletedAt == null).FirstOrDefaultAsync(s => s.Id == id);
            if (furniture is null) return NotFound("Bu sekil tapilmadi");

            return View(furniture);
        }

        // GET: FurnitureController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FurnitureController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Furniture furniture)
        {
            try
            {
                if (furniture.File == null)
                {
                    ModelState.AddModelError("File", "File is required");
                }

                (int status, string message) = await _fileService.FileUpload("furnitures", furniture.File);
                if (status == 0)
                {
                    ModelState.AddModelError("File", message);
                }

                if (!ModelState.IsValid)
                {
                    return View(furniture);
                }

                furniture.Image = message;

                await _context.Furnitures.AddAsync(furniture);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Furniture has ben created successfully";

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FurnitureController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            Furniture furniture = await _context.Furnitures.Where(x => x.DeletedAt == null).FirstOrDefaultAsync(f => f.Id == id);
            if (furniture == null) return NotFound();

            return View();
        }

        // POST: FurnitureController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Furniture model)
        {
            try
            {
                Furniture furniture = await _context.Furnitures.Where(x => x.DeletedAt == null).FirstOrDefaultAsync(f => f.Id == id);
                if (furniture == null) return NotFound();
                if (model.File != null)
                {
                    (int status, string message) = await _fileService.FileUpload("furnitures", model.File);
                    if (status == 0)
                    {
                        ModelState.AddModelError("File", message);
                        return View(furniture);
                    }

                    _fileService.DeleteFile("furnitures", furniture.Image);
                    furniture.Image = message;
                }

                furniture.Name = model.Name;
                furniture.Price = model.Price;
                furniture.UpdatedAt = DateAndTime.Now;
                await _context.SaveChangesAsync();
                TempData["Message"] = "Furniture has ben updated successfully";

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                Furniture furniture = await _context.Furnitures.FirstOrDefaultAsync(f => f.Id == id);
                if (furniture == null) return NotFound();

                furniture.DeletedAt = DateAndTime.Now;
                _fileService.DeleteFile("furnitures", furniture.Image);
                await _context.SaveChangesAsync();

                return Json(new
                {
                    Status = true,
                    Message = "Furniture has been deleted"
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
