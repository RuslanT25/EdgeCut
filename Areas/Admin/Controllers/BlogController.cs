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
    public class BlogController : Controller
    {
        readonly ApplicationContext _context;
        readonly IFileService _fileService;
        public BlogController(ApplicationContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }
        // GET: BlogController
        public async Task<IActionResult> Index()
        {
            List<Blog> blogs = await _context.Blogs.Where(x => x.DeletedAt == null).ToListAsync();

            return View(blogs);
        }

        // GET: BlogController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            Blog blog = await _context.Blogs.Where(x => x.DeletedAt == null).FirstOrDefaultAsync(s => s.Id == id);
            if (blog is null) return NotFound("Bu sekil tapilmadi");

            return View(blog);
        }

        // GET: BlogController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BlogController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog blog)
        {
            try
            {
                if (blog.File == null)
                {
                    ModelState.AddModelError("File", "File is required");
                }

                (int status, string message) = await _fileService.FileUpload("blogs", blog.File);
                if (status == 0)
                {
                    ModelState.AddModelError("File", message);
                }

                if (!ModelState.IsValid)
                {
                    return View(blog);
                }

                blog.Image = message;

                await _context.Blogs.AddAsync(blog);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Blog has ben created successfully";

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
            Blog blog = await _context.Blogs.Where(x => x.DeletedAt == null).FirstOrDefaultAsync(s => s.Id == id);
            if (blog is null) return NotFound("Bu sekil tapilmadi");

            return View(blog);
        }

        // POST: BlogController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Blog model)
        {
            try
            {
                Blog blog = await _context.Blogs.Where(x => x.DeletedAt == null).FirstOrDefaultAsync(f => f.Id == id);
                if (blog == null) return NotFound();
                if (model.File != null)
                {
                    (int status, string message) = await _fileService.FileUpload("blogs", model.File);
                    if (status == 0)
                    {
                        ModelState.AddModelError("File", message);
                        return View(blog);
                    }

                    _fileService.DeleteFile("blogs", blog.Image);
                    blog.Image = message;
                }

                blog.Title = model.Title;
                blog.Description = model.Description;
                blog.UpdatedAt = DateAndTime.Now;
                await _context.SaveChangesAsync();
                TempData["Message"] = "Blog has ben updated successfully";

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
                Blog blog = await _context.Blogs.FirstOrDefaultAsync(f => f.Id == id);
                if (blog == null) return NotFound();

                blog.DeletedAt = DateAndTime.Now;
                _fileService.DeleteFile("blogs", blog.Image);
                await _context.SaveChangesAsync();

                return Json(new
                {
                    Status = true,
                    Message = "Blog has been deleted"
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
