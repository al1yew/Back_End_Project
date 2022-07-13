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
    [Area("Manage")]
    //prosmotret vse v manage i pozirit nesostikovki
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
            IQueryable<Product> query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.ProductToColors).ThenInclude(p => p.Color)
                .Include(p => p.ProductToSizes).ThenInclude(p => p.Size);

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
            //burda ishleyir iki dene viewbag!!!!!!
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
                ModelState.AddModelError("Name", $"{product.Name} already exists");
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
            {//burda content type duzeldmek gesheng olsun
                if (!product.Photo.CheckContentType("image/jpeg")
                    && !product.Photo.CheckContentType("image/jpg")
                    && !product.Photo.CheckContentType("image/png")
                    && !product.Photo.CheckContentType("image/gif"))
                {
                    ModelState.AddModelError("Photo", "Main image must be jpg(jpeg) format");
                    return View();
                }

                if (product.Photo.CheckFileLength(15000))
                {
                    ModelState.AddModelError("Photo", "Main image size must be at most 15MB");
                    return View();
                }

                product.Image = await product.Photo.CreateAsync(_env, "assets", "img", "product");
            }
            else
            {
                ModelState.AddModelError("Photo", "Main image is required");
                return View();
            }

            if (product.Files != null && product.Files.Count() > 0)
            {
                List<ProductImage> productImages = new List<ProductImage>();

                foreach (IFormFile file in product.Files)
                {//burda content type duzeldmek gesheng olsun
                    if (!file.CheckContentType("image/jpeg")
                    && !file.CheckContentType("image/jpg")
                    && !file.CheckContentType("image/png")
                    && !file.CheckContentType("image/gif"))
                    {
                        ModelState.AddModelError("Files", "Main image must be image format");
                        return View();
                    }

                    if (file.CheckFileLength(15000))
                    {
                        ModelState.AddModelError("Files", "Each image's size must be at most 15MB");
                        return View();
                    }

                    ProductImage productImage = new ProductImage
                    {
                        Image = await file.CreateAsync(_env, "assets", "images", "product-slider-imgs")
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


        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            Product product = await _context.Products.Include(p => p.ProductImages).FirstOrDefaultAsync(c => !c.IsDeleted && c.Id == id);

            if (product == null) return NotFound();

            ViewBag.BrandsForProducts = await _context.Brands.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(c => !c.IsDeleted && !c.IsMain).ToListAsync();

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, Product product)
        {
            ViewBag.BrandsForProducts = await _context.Brands.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(c => !c.IsDeleted && !c.IsMain).ToListAsync();

            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();

            if (id != product.Id) return BadRequest();

            Product dbProduct = await _context.Products.Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (dbProduct == null) return NotFound();

            if (!await _context.Brands.AnyAsync(b => !b.IsDeleted && b.Id == product.BrandId))
            {
                ModelState.AddModelError("BrandId", "Select brand");
                return View(product);
            }

            if (product.CategoryId == null && !await _context.Categories.AnyAsync(c => !c.IsDeleted && !c.IsMain && c.Id == product.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Select category");
                return View(product);
            }

            int canSelectCount = 5 - dbProduct.ProductImages.Count();

            if (product.Files != null && canSelectCount < product.Files.Count())
            {
                ModelState.AddModelError("Files", $"You can select {canSelectCount} items");
                return View(product);
            }

            if (product.Photo != null)
            {
                if (!product.Photo.CheckContentType("image/jpeg")
                    && !product.Photo.CheckContentType("image/jpg")
                    && !product.Photo.CheckContentType("image/png")
                    && !product.Photo.CheckContentType("image/gif"))
                {
                    ModelState.AddModelError("Photo", "Image must be image format");
                    return View(product);
                }

                if (product.Photo.CheckFileLength(15000))
                {
                    ModelState.AddModelError("Photo", "Image size must be at most 15MB");
                    return View(product);
                }

                FileHelper.DeleteFile(_env, dbProduct.Image, "assets", "img", "product");

                dbProduct.Image = await product.Photo.CreateAsync(_env, "assets", "img", "product");
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
                        ModelState.AddModelError("Files", "Images must be image format");
                        return View(product);
                    }

                    if (file.CheckFileLength(15000))
                    {
                        ModelState.AddModelError("Files", "Each image's size must be at most 15MB");
                        return View(product);
                    }

                    ProductImage productImage = new ProductImage
                    {
                        Image = await file.CreateAsync(_env, "assets", "images", "product-slider-imgs")
                    };

                    productImages.Add(productImage);
                }

                dbProduct.ProductImages.AddRange(productImages);
            }

            dbProduct.Name = product.Name.Trim();

            await _context.SaveChangesAsync();

            TempData["success"] = "Product Is Updated!";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id, int? status, int select, int page)
        {
            if (id == null) return BadRequest();

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (product == null) return NotFound();

            product.IsDeleted = true;
            product.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

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

            return PartialView("_ProductIndexPartial", PaginationList<Product>.Create(query, page, select));
        }

        public async Task<IActionResult> Restore(int? id, int? status, int select, int page)
        {
            if (id == null) return BadRequest();

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            product.IsDeleted = false;
            product.DeletedAt = null;

            await _context.SaveChangesAsync();

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

            return PartialView("_ProductIndexPartial", PaginationList<Product>.Create(query, page, select));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteImage(int? id)
        {
            ViewBag.BrandsForProducts = await _context.Brands.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(c => !c.IsDeleted && !c.IsMain).ToListAsync();

            if (id == null) return BadRequest();

            ProductImage productImage = await _context.ProductImages.FirstOrDefaultAsync(p => p.Id == id);

            if (productImage == null) return NotFound();

            Product product = await _context.Products.Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.Id == productImage.ProductId);

            _context.ProductImages.Remove(productImage);

            await _context.SaveChangesAsync();

            FileHelper.DeleteFile(_env, productImage.Image, "assets", "img", "product-slider-imgs");

            return PartialView("_ProductImagePartial", product.ProductImages);
        }
    }
}
