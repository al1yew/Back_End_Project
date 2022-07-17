using Back_End_Project.DAL;
using Back_End_Project.Models;
using Back_End_Project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class BlogTagController : Controller
    {
        private readonly AppDbContext _context;
        public BlogTagController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? status, int select, int page = 1)
        {
            IQueryable<BlogTag> query = _context.BlogTags;

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

            return View(PaginationList<BlogTag>.Create(query, page, select));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogTag blogTag)
        {
            if (!ModelState.IsValid) return View();

            if (await _context.BlogTags.AnyAsync(b => b.Name.ToLower().Trim() == blogTag.Name.ToLower().Trim() && !b.IsDeleted))
            {
                ModelState.AddModelError("Name", $"{blogTag.Name} already exists");
                return View();
            }

            blogTag.CreatedAt = DateTime.UtcNow.AddHours(+4);

            await _context.BlogTags.AddAsync(blogTag);
            await _context.SaveChangesAsync();

            TempData["success"] = "Tag Is Created";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            BlogTag blogTag = await _context.BlogTags.FirstOrDefaultAsync(b => b.Id == id);

            if (blogTag == null) return NotFound();

            return View(blogTag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, BlogTag blogTag)
        {
            if (id == null) return BadRequest();

            if (id != blogTag.Id) return BadRequest();

            BlogTag dbBlogTag = await _context.BlogTags.FirstOrDefaultAsync(b => b.Id == blogTag.Id);

            if (dbBlogTag == null) return NotFound();

            if (await _context.BlogTags.AnyAsync(b => b.Id != blogTag.Id && !b.IsDeleted && b.Name.ToLower().Trim() == blogTag.Name.ToLower().Trim()))
            {
                ModelState.AddModelError("Name", $"{blogTag.Name} already exists");
                return View();
            }

            dbBlogTag.Name = blogTag.Name;
            dbBlogTag.IsUpdated = true;
            dbBlogTag.UpdatedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            TempData["success"] = "Tag Is Updated";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id, int? status, int select, int page)
        {
            if (id == null) return BadRequest();

            BlogTag blogTag = await _context.BlogTags.FirstOrDefaultAsync(b => b.Id == id);

            if (blogTag == null) return NotFound();

            blogTag.IsDeleted = true;
            blogTag.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            IQueryable<BlogTag> query = _context.BlogTags;

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

            TempData["success"] = "Tag is deleted";

            return PartialView("_BlogTagIndexPartial", PaginationList<BlogTag>.Create(query, page, select));
        }

        public async Task<IActionResult> Restore(int? id, int? status, int select, int page)
        {
            if (id == null) return BadRequest();

            BlogTag blogTag = await _context.BlogTags.FirstOrDefaultAsync(b => b.Id == id);

            if (blogTag == null) return NotFound();

            blogTag.IsDeleted = false;
            blogTag.DeletedAt = null;

            await _context.SaveChangesAsync();

            IQueryable<BlogTag> query = _context.BlogTags;

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

            TempData["success"] = "Tag is restored";

            return PartialView("_BlogTagIndexPartial", PaginationList<BlogTag>.Create(query, page, select));
        }

    }
}
