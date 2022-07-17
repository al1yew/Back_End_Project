using Back_End_Project.DAL;
using Back_End_Project.Models;
using Back_End_Project.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class BlogCategoryController : Controller
    {
        private readonly AppDbContext _context;
        public BlogCategoryController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? status, int select, int page = 1)
        {
            IQueryable<BlogCategory> query = _context.BlogCategories;

            if (status != null && status > 0)
            {
                if (status == 1)
                {
                    query = query.Where(p => p.IsDeleted);
                }
                else if (status == 2)
                {
                    query = query.Where(p => !p.IsDeleted);
                }
            }

            if (select <= 0)
            {
                select = 5;
            }

            ViewBag.Select = select;

            ViewBag.Status = status;

            return View(PaginationList<BlogCategory>.Create(query, page, select));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCategory blogCategory)
        {
            if (!ModelState.IsValid) return View();

            if (await _context.BlogCategories.AnyAsync(b => b.Name.ToLower().Trim() == blogCategory.Name.ToLower().Trim() && !b.IsDeleted))
            {
                ModelState.AddModelError("Name", $"{blogCategory.Name} already exists");
                return View();
            }

            blogCategory.CreatedAt = DateTime.UtcNow.AddHours(+4);

            await _context.BlogCategories.AddAsync(blogCategory);
            await _context.SaveChangesAsync();

            TempData["success"] = "Category Is Created";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            BlogCategory blogCategory = await _context.BlogCategories.FirstOrDefaultAsync(b => b.Id == id);

            if (blogCategory == null) return NotFound();

            return View(blogCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, BlogCategory blogCategory)
        {
            if (id == null) return BadRequest();

            if (id != blogCategory.Id) return BadRequest();

            BlogCategory dbBlogCategory = await _context.BlogCategories.FirstOrDefaultAsync(b => b.Id == blogCategory.Id);

            if (dbBlogCategory == null) return NotFound();

            if (await _context.BlogCategories.AnyAsync(b => b.Id != blogCategory.Id && !b.IsDeleted && b.Name.ToLower().Trim() == blogCategory.Name.ToLower().Trim()))
            {
                ModelState.AddModelError("Name", $"{blogCategory.Name} already exists");
                return View();
            }

            dbBlogCategory.Name = blogCategory.Name;
            dbBlogCategory.IsUpdated = true;
            dbBlogCategory.UpdatedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            TempData["success"] = "Category Is Updated";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id, int? status, int select, int page)
        {
            if (id == null) return BadRequest();

            BlogCategory blogCategory = await _context.BlogCategories.FirstOrDefaultAsync(b => b.Id == id);

            if (blogCategory == null) return NotFound();

            blogCategory.IsDeleted = true;
            blogCategory.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            IQueryable<BlogCategory> query = _context.BlogCategories;

            if (status != null && status > 0)
            {
                if (status == 1)
                {
                    query = query.Where(b => b.IsDeleted);
                }
                else if (status == 2)
                {
                    query = query.Where(b => !b.IsDeleted);
                }
            }

            if (select <= 0)
            {
                select = 5;
            }

            ViewBag.Select = select;

            ViewBag.Status = status;

            TempData["success"] = "Category is deleted";

            return PartialView("_BlogCategoryIndexPartial", PaginationList<BlogCategory>.Create(query, page, select));
        }

        public async Task<IActionResult> Restore(int? id, int? status, int select, int page)
        {
            if (id == null) return BadRequest();

            BlogCategory blogCategory = await _context.BlogCategories.FirstOrDefaultAsync(b => b.Id == id);

            if (blogCategory == null) return NotFound();

            blogCategory.IsDeleted = false;
            blogCategory.DeletedAt = null;

            await _context.SaveChangesAsync();

            IQueryable<BlogCategory> query = _context.BlogCategories;

            if (status != null && status > 0)
            {
                if (status == 1)
                {
                    query = query.Where(b => b.IsDeleted);
                }
                else if (status == 2)
                {
                    query = query.Where(b => !b.IsDeleted);
                }
            }

            if (select <= 0)
            {
                select = 5;
            }

            ViewBag.Select = select;

            ViewBag.Status = status;

            TempData["success"] = "Category is restored";

            return PartialView("_BlogCategoryIndexPartial", PaginationList<BlogCategory>.Create(query, page, select));
        }
    }
}
