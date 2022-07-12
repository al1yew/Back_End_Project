using Back_End_Project.DAL;
using Back_End_Project.Models;
using Back_End_Project.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End_Project.Helper;
using Back_End_Project.Extensions;
namespace Back_End_Project.Areas.Manage.Controllers
{
    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int? status, int select, int page = 1)
        {
            IQueryable<Product> query = _context.Products.Include(p => p.Category).Include(p => p.Brand);

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

            return View(PaginationList<Product>.Create(query, page, select));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.BrandsForProducts = await _context.Brands.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(c => !c.IsDeleted && !c.IsMain).ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            ViewBag.BrandsForProducts = await _context.Brands.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(c => !c.IsDeleted && !c.IsMain).ToListAsync();

            if (!ModelState.IsValid) return View();

            if (await _context.Products.AnyAsync(p => p.Name.ToLower().Trim() == product.Name.ToLower().Trim() && !p.IsDeleted))
            {
                ModelState.AddModelError("ProductName", $"{product.Name} already exists");
                return View();
            }

            if (!await _context.Brands.AnyAsync(b => !b.IsDeleted && b.Id == product.BrandId))
            {
                ModelState.AddModelError("BrandId", "Select brand");
                return View();
            }

            if (product.CategoryId == null && !await _context.Categories.AnyAsync(c => !c.IsDeleted && !c.IsMain && c.Id == product.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Select category");
                return View();
            }

            if (product.Files != null && product.Files.Count() > 5)
            {
                ModelState.AddModelError("Files", "You can select maximum 5 images");
                return View();
            }

            if (product.Image != null)
            {
                if (!product.Photo.CheckContentType("image/jpeg")
                    && !product.Photo.CheckContentType("image/jpg")
                    && !product.Photo.CheckContentType("image/png")
                    && !product.Photo.CheckContentType("image/gif"))
                {
                    ModelState.AddModelError("MainFile", "Main image must be jpg(jpeg) format");
                    return View();
                }

                if (product.Photo.CheckFileLength(15000))
                {
                    ModelState.AddModelError("MainFile", "Main image size must be 15MB");
                    return View();
                }

                product.Image = await product.Photo.CreateAsync(_env, "assets", "images");
            }
            else
            {
                ModelState.AddModelError("MainFile", "Main image is required");
                return View();
            }

            if (product.Files != null && product.Files.Count() > 0)
            {
                List<ProductImage> productImages = new List<ProductImage>();

                foreach (IFormFile file in product.Files)
                {
                    if (!file.CheckContentType("image/jpeg")
                    && !file.CheckContentType("image/jpg")
                    && !file.CheckContentType("image/png")
                    && !file.CheckContentType("image/gif"))
                    {
                        ModelState.AddModelError("MainFile", "Main image must be jpg(jpeg) format");
                        return View();
                    }

                    if (file.CheckFileLength(15000))
                    {
                        ModelState.AddModelError("MainFile", "Main image size must be 15MB");
                        return View();
                    }

                    ProductImage productImage = new ProductImage
                    {
                        Image = await file.CreateAsync(_env, "assets", "images")
                    };

                    productImages.Add(productImage);
                }

                product.ProductImages = productImages;
            }

            product.Name = product.Name.Trim();

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            TempData["success"] = "Product Is Created!";

            return RedirectToAction("Index");
        }
    }
}
