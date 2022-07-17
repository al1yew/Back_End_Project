
using Back_End_Project.DAL;
using Back_End_Project.Extensions;
using Back_End_Project.Helper;
using Back_End_Project.Models;
using Back_End_Project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Areas.Manage.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    [Area("Manage")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public CategoryController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index(int? status, int? type, int select, int page = 1)
        {
            IQueryable<Category> query = _context.Categories;

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

            if (type != null && type > 0)
            {
                if (type == 1)
                {
                    query = query.Where(c => !c.IsMain);
                }
                else if (type == 2)
                {
                    query = query.Where(c => c.IsMain);
                }
            }

            if (select <= 0)
            {
                select = 5;
            }

            ViewBag.Select = select;

            ViewBag.Status = status;

            ViewBag.Type = type;

            return View(PaginationList<Category>.Create(query, page, select));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.MainCategories = await _context.Categories.Where(c => !c.IsDeleted && c.IsMain).ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            ViewBag.MainCategories = await _context.Categories.Where(c => !c.IsDeleted && c.IsMain).ToListAsync();

            if (!ModelState.IsValid) return View();

            if (category.IsMain)
            {
                if (category.Photo == null)
                {
                    ModelState.AddModelError("Photo", "Image is required in main category");
                    return View();
                }
            }
            else
            {
                if (category.ParentId == null || !await _context.Categories.AnyAsync(c => !c.IsDeleted && c.IsMain && c.Id == category.ParentId))
                {
                    ModelState.AddModelError("ParentId", "Parent is chosen wrongly");
                    return View();
                }
            }

            if (await _context.Categories.AnyAsync(c => !c.IsDeleted && c.Name == category.Name.Trim().ToLower()))
            {
                ModelState.AddModelError("Name", "Already Exist");
                return View();
            }

            if (category.Photo != null)
            {
                if (!category.Photo.CheckContentType("image/jpeg")
                    && !category.Photo.CheckContentType("image/jpg")
                    && !category.Photo.CheckContentType("image/png")
                    && !category.Photo.CheckContentType("image/gif"))
                {
                    ModelState.AddModelError("File", "You can choose only Image format!");
                    return View();
                }

                if (category.Photo.CheckFileLength(15000))
                {
                    ModelState.AddModelError("File", "File must be 15MB at most!");
                    return View();
                }

                category.Image = await category.Photo.CreateAsync(_env, "assets", "img", "product");
            }

            category.Name = category.Name.Trim();
            category.CreatedAt = DateTime.UtcNow.AddHours(4);

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            TempData["success"] = "Category is created";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            Category category = await _context.Categories.FirstOrDefaultAsync(c => !c.IsDeleted && c.Id == id);

            if (category == null) return NotFound();

            ViewBag.MainCategories = await _context.Categories.Where(c => !c.IsDeleted && c.IsMain).ToListAsync();

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, Category category)
        {
            ViewBag.MainCategories = await _context.Categories.Where(c => !c.IsDeleted && c.IsMain).ToListAsync();

            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();

            if (id != category.Id) return BadRequest();

            if (!category.IsMain)
            {
                if (category.ParentId == null || !await _context.Categories.AnyAsync(c => c.Id == category.ParentId && c.IsMain && !c.IsDeleted))
                {
                    ModelState.AddModelError("ParentId", "Parent category is required!");
                    return View(category);
                }
            }

            Category dbCategory = await _context.Categories.FirstOrDefaultAsync(c => !c.IsDeleted && c.Id == category.Id);

            if (dbCategory == null) return NotFound();

            if (await _context.Categories.AnyAsync(c => c.Id != category.Id && c.Name.ToLower() == category.Name.Trim().ToLower()))
            {
                ModelState.AddModelError("Name", "Already exists!");
                return View(category);
            }

            if (category.Photo != null)
            {
                if (!category.Photo.CheckContentType("image/jpeg")
                    && !category.Photo.CheckContentType("image/jpg")
                    && !category.Photo.CheckContentType("image/png")
                    && !category.Photo.CheckContentType("image/gif"))
                {
                    ModelState.AddModelError("Photo", "You can choose only Image format!");
                    return View();
                }

                if (category.Photo.CheckFileLength(15000))
                {
                    ModelState.AddModelError("File", "File must be 15MB at most!");
                    return View();
                }

                FileHelper.DeleteFile(_env, dbCategory.Image, "assets", "img", "product");

                dbCategory.Image = await category.Photo.CreateAsync(_env, "assets", "img", "product");
            }

            dbCategory.Name = category.Name.Trim();
            dbCategory.IsMain = category.IsMain;
            dbCategory.ParentId = category.IsMain ? null : category.ParentId;
            dbCategory.IsUpdated = true;
            dbCategory.UpdatedAt = DateTime.UtcNow.AddHours(4);
            await _context.SaveChangesAsync();

            TempData["success"] = "Category is updated";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id, int? status, int? type, int select, int page = 1)
        {
            if (id == null) return BadRequest();

            Category category = await _context.Categories.FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);

            if (category == null) return NotFound();

            if (category.IsMain)
            {
                List<Category> children = await _context.Categories.Where(c => c.ParentId == category.Id && !c.IsDeleted).ToListAsync();

                foreach (Category child in children)
                {
                    child.IsDeleted = true;
                    child.DeletedAt = DateTime.UtcNow.AddHours(4);
                }
            }

            category.IsDeleted = true;
            category.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            IQueryable<Category> query = _context.Categories;

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

            if (type != null && type > 0)
            {
                if (type == 1)
                {
                    query = query.Where(c => !c.IsMain);
                }
                else if (type == 2)
                {
                    query = query.Where(c => c.IsMain);
                }
            }

            if (select <= 0)
            {
                select = 5;
            }

            ViewBag.Select = select;

            ViewBag.Status = status;

            ViewBag.Type = type;

            TempData["success"] = "Category is deleted";

            return PartialView("_CategoryIndexPartial", PaginationList<Category>.Create(query, page, select));
        }

        public async Task<IActionResult> Restore(int? id, int? status, int? type, int select, int page = 1)
        {
            if (id == null) return BadRequest();

            Category category = await _context.Categories.FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);

            if (category == null) return NotFound();

            if (!category.IsMain && await _context.Categories.AnyAsync(c => c.Id == category.ParentId && c.IsDeleted))
            {
                return BadRequest();
            }

            category.IsDeleted = false;
            category.DeletedAt = null;

            await _context.SaveChangesAsync();

            IQueryable<Category> query = _context.Categories;

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

            if (type != null && type > 0)
            {
                if (type == 1)
                {
                    query = query.Where(c => !c.IsMain);
                }
                else if (type == 2)
                {
                    query = query.Where(c => c.IsMain);
                }
            }

            if (select <= 0)
            {
                select = 5;
            }

            ViewBag.Select = select;

            ViewBag.Status = status;

            ViewBag.Type = type;
            
            TempData["success"] = "Category is restored";

            return PartialView("_CategoryIndexPartial", PaginationList<Category>.Create(query, page, select));
        }
    }
}
