
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
    public class BrandController : Controller
    {
        private readonly AppDbContext _context;

        public BrandController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? status, int page = 1)
        {
            IQueryable<Brand> query = _context.Brands;

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

            int itemCount = int.Parse(_context.Settings.FirstOrDefault(s => s.Key == "PageItemsCount").Value);

            ViewBag.Status = status;

            return View(PaginationList<Brand>.Create(query, page, itemCount));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Brand brand)
        {
            if (!ModelState.IsValid) return View();

            if (await _context.Brands.AnyAsync(b => b.Name.ToLower().Trim() == brand.Name.ToLower().Trim() && !b.IsDeleted))
            {
                ModelState.AddModelError("Name", $"{brand.Name} already exists");
                return View();
            }

            brand.CreatedAt = DateTime.UtcNow.AddHours(+4);

            await _context.Brands.AddAsync(brand);
            await _context.SaveChangesAsync();

            TempData["success"] = "Brand Is Created";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            Brand brand = await _context.Brands.FirstOrDefaultAsync(b => b.Id == id);

            if (brand == null) return NotFound();

            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Brand brand)
        {
            if (id == null) return BadRequest();

            if (id != brand.Id) return BadRequest();

            Brand dbBrand = await _context.Brands.FirstOrDefaultAsync(b => b.Id == brand.Id);

            if (dbBrand == null) return NotFound();

            if (await _context.Brands.AnyAsync(b => b.Id != brand.Id && !b.IsDeleted && b.Name.ToLower().Trim() == brand.Name.ToLower().Trim()))
            {
                ModelState.AddModelError("Name", $"{brand.Name} already exists");
                return View();
            }

            dbBrand.Name = brand.Name;
            dbBrand.IsUpdated = true;
            dbBrand.UpdatedAt = DateTime.UtcNow.AddHours(4);
            await _context.SaveChangesAsync();

            TempData["success"] = "Brand Is Updated";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id, int? status, int page)
        {
            if (id == null) return BadRequest();

            Brand brand = await _context.Brands.FirstOrDefaultAsync(b => b.Id == id);

            if (brand == null) return NotFound();
            //if we want to delete permanently also from db we write:
            //_context.Brands.Remove(brand);

            brand.IsDeleted = true;
            brand.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            IQueryable<Brand> query = _context.Brands;

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

            int itemCount = int.Parse(_context.Settings.FirstOrDefault(s => s.Key == "PageItemsCount").Value);

            ViewBag.Status = status;

            return PartialView("_BrandIndexPartial", PaginationList<Brand>.Create(query, page, itemCount));
        }

        public async Task<IActionResult> Restore(int? id, int? status, int page)
        {
            if (id == null) return BadRequest();

            Brand brand = await _context.Brands.FirstOrDefaultAsync(b => b.Id == id);

            if (brand == null) return NotFound();

            brand.IsDeleted = false;
            brand.DeletedAt = null;

            await _context.SaveChangesAsync();

            IQueryable<Brand> query = _context.Brands;

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

            ViewBag.Status = status;

            int pageCount = int.Parse(_context.Settings.FirstOrDefault(s => s.Key == "PageItemsCount").Value);

            return PartialView("_BrandIndexPartial", PaginationList<Brand>.Create(query, page, pageCount));
        }
    }
}
