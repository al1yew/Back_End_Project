using Back_End_Project.DAL;
using Back_End_Project.Models;
using Back_End_Project.ViewModels;
using Back_End_Project.ViewModels.BasketViewModels;
using Back_End_Project.ViewModels.ProductViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;

//sdelat sliderrange, on doljen otprravlat v metod po buttonu, ix mojno vzat v js
//i fetchanut ix kak dva rezultata, maxprice i minprice, i ya doljen sortirovat ix s pomoshyu etix dvux cen
//eto sdelano v detail page s pomoshyu rating 

//detail page xochu shto b s inputa value mojno bilo vpisivat rukoy
//product detail stroka 67 s viewbagami, ix iskat v basketcontrollere

//detail page related products modal photo slider ishlemir
//footer linkleri duzelt sagda

namespace Back_End_Project.Controllers
{
    public class ProductController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;
        public ProductController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int? sortby, int? categoryId, int? brandId, string searchValue, int? colorId, int? sizeId, int sortbycount, int page = 1)
        {
            IQueryable<Product> products = _context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Include(p => p.ProductToColors).ThenInclude(p => p.Color)
            .Include(p => p.ProductToSizes).ThenInclude(p => p.Size);

            if (categoryId != null)
            {
                products = products
                    .Where(p => p.CategoryId == categoryId);
            }

            if (brandId != null)
            {
                products = products
                    .Where(p => p.BrandId == brandId);
            }

            if (searchValue != null)
            {
                products = products
                .Where(p => p.Name.ToLower().Contains(searchValue.ToLower()) ||
                p.Brand.Name.ToLower().Contains(searchValue.ToLower()) ||
                p.Category.Name.ToLower().Contains(searchValue.ToLower()) ||
                p.Description.ToLower().Contains(searchValue.ToLower()) ||
                p.FirstText.ToLower().Contains(searchValue.ToLower()) ||
                p.SecondText.ToLower().Contains(searchValue.ToLower()));
            }

            if (colorId != null)
            {
                products = _context.ProductToColors
                   .Include(x => x.Product).Where(e => e.ColorId == colorId).Select(e => e.Product);
            }

            if (sizeId != null)
            {
                products = _context.ProductToSizes
                    .Include(x => x.Product).Where(e => e.SizeId == sizeId).Select(e => e.Product);
            }

            if (sortby != null && sortby > 0)
            {
                if (sortby == 1)
                {
                    products = products.OrderBy(c => c.Name);
                }
                else if (sortby == 2)
                {
                    products = products.OrderByDescending(c => c.Name);
                }
                else if (sortby == 3)
                {
                    products = products.OrderBy(c => c.Price);
                }
                else if (sortby == 4)
                {
                    products = products.OrderByDescending(c => c.Price);
                }
            }

            if (sortbycount <= 0)
            {
                sortbycount = 5;
            }

            ProductVM productVM = new ProductVM
            {
                Products = PaginationList<Product>.Create(products, page, sortbycount),
                Settings = await _context.Settings.ToDictionaryAsync(x => x.Key, x => x.Value),
                Sizes = await _context.Sizes.Include(c => c.ProductToSizes).ThenInclude(pc => pc.Product).ToListAsync(),
                Colors = await _context.Colors.Include(c => c.ProductToColors).ThenInclude(pc => pc.Product).ToListAsync(),
                Categories = await _context.Categories.Include(c => c.Products).ToListAsync(),
                Brands = await _context.Brands.Include(b => b.Products).ToListAsync()
            };

            ViewBag.CategoriesForProductsPage = categoryId;
            ViewBag.BrandsForProductsPage = brandId;
            ViewBag.HeaderSearchForProductsPage = searchValue;
            ViewBag.ColorForProductsPage = colorId;
            ViewBag.SizeForProductsPage = sizeId;
            ViewBag.Sortby = sortby;
            ViewBag.Select = sortbycount;

            //asp-route-categoryId="@ViewBag.CategoriesForProductsPage" asp-route-brandId="@ViewBag.BrandsForProductsPage" asp-route-searchValue="@ViewBag.HeaderSearchForProductsPage" asp-route-colorId="@ViewBag.ColorForProductsPage" asp-route-sizeId="@ViewBag.SizeForProductsPage" asp-route-sortby="@ViewBag.Sortby" asp-route-sortbycount="@ViewBag.Select"

            return View(productVM);
        }

        //[Authorize(Roles = "Member")]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name && !u.IsAdmin);

            if (appUser == null) return RedirectToAction("LoginRegister", "Account");

            Product product = await _context.Products
                .Include(p => p.ProductToColors).ThenInclude(p => p.Color)
                .Include(p => p.ProductToSizes).ThenInclude(p => p.Size)
                .Include(p => p.ProductImages)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductInformation)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            ProductReviewVM productReviewVM = new ProductReviewVM
            {
                Name = appUser.Name,
                Email = appUser.Email,
                AppUserId = appUser.Id
            };

            ProductDetailVM productDetailVM = new ProductDetailVM
            {
                Product = product,
                Products = await _context.Products.ToListAsync(),
                ProductReviewVM = productReviewVM,
                ProductReviews = await _context.ProductReviews.Where(pr => pr.ProductId == id).ToListAsync()
            };

            ViewBag.ProductId = id;

            return View(productDetailVM);
        }

        public async Task<IActionResult> DetailModal(int? id)
        {
            if (id == null) return BadRequest();

            Product product = await _context.Products
                 .Include(p => p.ProductToColors).ThenInclude(p => p.Color)
                 .Include(p => p.ProductToSizes).ThenInclude(p => p.Size)
                 .Include(p => p.ProductImages)
                 .Include(p => p.Brand)
                 .Include(p => p.Category)
                 .Include(p => p.ProductInformation)
                 .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            return PartialView("_ProductModalPartial", product);
        }

        public async Task<IActionResult> Search(string search)
        {
            List<Product> products = await _context.Products
                .Where(p => p.Name.ToLower().Contains(search.ToLower()) ||
                p.Brand.Name.ToLower().Contains(search.ToLower()) ||
                p.Category.Name.ToLower().Contains(search.ToLower()) ||
                p.Description.ToLower().Contains(search.ToLower()) ||
                p.FirstText.ToLower().Contains(search.ToLower()) ||
                p.SecondText.ToLower().Contains(search.ToLower()))
                .ToListAsync();

            return PartialView("_SearchPartial", products);
        }

        [HttpPost]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> AddProductReview(ProductReviewVM productReviewVM, int id)
        {
            if (!ModelState.IsValid) return Redirect($"http://localhost:15866/Product/Detail/{id}");

            if (id == null && id <= 0) return NotFound();

            if (await _context.ProductReviews.Where(x=>x.ProductId == productReviewVM.ProductId).AnyAsync(pr => pr.Email.Trim().ToLower() == productReviewVM.Email.Trim().ToLower()))
            {
                ModelState.AddModelError("", "You have already placed your review!");
                return Redirect($"http://localhost:15866/Product/Detail/{id}");
            }

            ProductReview productReview = new ProductReview
            {
                CreatedAt = DateTime.UtcNow.AddHours(4),
                Email = productReviewVM.Email,
                Name = productReviewVM.Name,
                Rating = productReviewVM.Rating,
                ReviewText = productReviewVM.ReviewText,
                ProductId = id,
                AppUserId = productReviewVM.AppUserId,
            };

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            product.ReviewCount++;

            //await _context.Products.AddAsync(product);
            await _context.ProductReviews.AddAsync(productReview);
            await _context.SaveChangesAsync();

            TempData["success"] = "Thanks for review!";

            return Redirect($"http://localhost:15866/Product/Detail/{id}");
        }
    }
}
