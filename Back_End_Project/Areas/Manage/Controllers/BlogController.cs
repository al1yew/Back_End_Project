using Back_End_Project.DAL;
using Back_End_Project.Models;
using Back_End_Project.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//hele mene blogtag ve blogcategory crudu lazimdi
namespace Back_End_Project.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class BlogController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;

        public BlogController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int? status, int select, int page = 1)
        {
            IQueryable<Blog> query = _context.Blogs
                .Include(p => p.BlogCategory)
                .Include(p => p.BlogAuthor)
                .Include(p => p.BlogComments).ThenInclude(p => p.BlogCommentReplies)
                .Include(p => p.BlogTag);

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

            return View(PaginationList<Blog>.Create(query, page, select));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            //ViewBag.Tags = await _context.BlogTags.Where(b => !b.IsDeleted).ToListAsync();
            //ViewBag.Categories = await _context.BlogCategories.Where(c => !c.IsDeleted && !c.IsMain).ToListAsync();

            ViewBag.Tags = await _context.BlogTags.ToListAsync();
            ViewBag.Categories = await _context.BlogCategories.ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog blog)
        {
            ViewBag.Tags = await _context.BlogTags.ToListAsync();
            ViewBag.Categories = await _context.BlogCategories.ToListAsync();

            if (!ModelState.IsValid) return View();

            if (!await _context.BlogTags.AnyAsync(b => !b.IsDeleted && b.Id == blog.BlogTagId))
            {
                ModelState.AddModelError("BlogTagId", "Select Tag!");
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

            if (product.Photo != null)
            {
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
                        Image = await file.CreateAsync(_env, "assets", "img", "product-slider-imgs")
                    };

                    productImages.Add(productImage);
                }

                product.ProductImages = productImages;
            }

            product.Name = product.Name.Trim();

            await _context.Products.AddAsync(product);

            product.CreatedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            TempData["success"] = "Product Is Created!";

            return RedirectToAction("Index");
        }
    }
}
