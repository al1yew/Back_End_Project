using Back_End_Project.DAL;
using Back_End_Project.Models;
using Back_End_Project.ViewModels.ProductViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//v indekse nujno popravit (say), sdelat sliderrange, on doljen otprravlat v metod po buttonu, ix mojno vzat v js i fetchanut ix kak
//dva rezultata, maxprice i minprice, i ya doljen sortirovat ix s pomoshyu etix dvux cen
//sdelat normalnie produkti nakonecto
//sdelat sort by relevance,
//sdelat pagination,
//sdelat sort by 5 10 15 20 shtuk
//Showing 1–16 Of 21 Results toje nado popravit

namespace Back_End_Project.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Product> products = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.ProductToColors).ThenInclude(p => p.Color)
            .Include(p => p.ProductToSizes).ThenInclude(p => p.Size)
            .ToListAsync();

            ProductVM productVM = new ProductVM
            {
                Products = products,
                Settings = await _context.Settings.ToDictionaryAsync(x => x.Key, x => x.Value),
                Sizes = await _context.Sizes.ToListAsync(),
                Colors = await _context.Colors.ToListAsync(),
                Categories = await _context.Categories.ToListAsync()
            };

            return View(productVM);
        }

        public async Task<IActionResult> Detail(int? id)
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

            ProductDetailVM productDetailVM = new ProductDetailVM
            {
                Product = product,
                Products = await _context.Products.ToListAsync()
            };

            if (product == null) return NotFound();

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

        public async Task<IActionResult> SortByCategory(int? id)
        {
            if (id == null)
            {
                List<Product> products = await _context.Products.ToListAsync();

                ProductVM productVM = new ProductVM
                {
                    Products = products,
                    Settings = await _context.Settings.ToDictionaryAsync(x => x.Key, x => x.Value),
                    Sizes = await _context.Sizes.ToListAsync(),
                    Colors = await _context.Colors.ToListAsync(),
                    Categories = await _context.Categories.ToListAsync()
                };

                return View("Index", productVM);
            }
            else
            {
                List<Product> products = await _context.Products
                    .Where(p => p.CategoryId == id)
                    .ToListAsync();

                ProductVM productVM = new ProductVM
                {
                    Products = products,
                    Settings = await _context.Settings.ToDictionaryAsync(x => x.Key, x => x.Value),
                    Sizes = await _context.Sizes.ToListAsync(),
                    Colors = await _context.Colors.ToListAsync(),
                    Categories = await _context.Categories.ToListAsync()
                };

                return View("Index", productVM);
            }

        }

        public async Task<IActionResult> SortByBrand(int? id)
        {
            if (id == null)
            {
                List<Product> products = await _context.Products.ToListAsync();

                ProductVM productVM = new ProductVM
                {
                    Products = products,
                    Settings = await _context.Settings.ToDictionaryAsync(x => x.Key, x => x.Value),
                    Sizes = await _context.Sizes.ToListAsync(),
                    Colors = await _context.Colors.ToListAsync(),
                    Categories = await _context.Categories.ToListAsync()
                };

                return View("Index", productVM);
            }
            else
            {
                List<Product> products = await _context.Products
                    .Where(p => p.BrandId == id)
                    .ToListAsync();

                ProductVM productVM = new ProductVM
                {
                    Products = products,
                    Settings = await _context.Settings.ToDictionaryAsync(x => x.Key, x => x.Value),
                    Sizes = await _context.Sizes.ToListAsync(),
                    Colors = await _context.Colors.ToListAsync(),
                    Categories = await _context.Categories.ToListAsync()
                };

                return View("Index", productVM);
            }
        }

        public async Task<IActionResult> SortByColor(int? id)
        {
            if (id == null)
            {
                List<Product> products = await _context.Products.ToListAsync();

                ProductVM productVM = new ProductVM
                {
                    Products = products,
                    Settings = await _context.Settings.ToDictionaryAsync(x => x.Key, x => x.Value),
                    Sizes = await _context.Sizes.ToListAsync(),
                    Colors = await _context.Colors.ToListAsync(),
                    Categories = await _context.Categories.ToListAsync()
                };

                return View("Index", productVM);
            }
            else
            {
                List<Product> products = await _context.ProductToColors
                    .Include(x => x.Product).Where(e => e.ColorId == id).Select(e => e.Product)
                    .ToListAsync();

                ProductVM productVM = new ProductVM
                {
                    Products = products,
                    Settings = await _context.Settings.ToDictionaryAsync(x => x.Key, x => x.Value),
                    Sizes = await _context.Sizes.ToListAsync(),
                    Colors = await _context.Colors.ToListAsync(),
                    Categories = await _context.Categories.ToListAsync()
                };

                return View("Index", productVM);
            }
        }

        public async Task<IActionResult> SortBySize(int? id)
        {
            if (id == null)
            {
                List<Product> products = await _context.Products.ToListAsync();

                ProductVM productVM = new ProductVM
                {
                    Products = products,
                    Settings = await _context.Settings.ToDictionaryAsync(x => x.Key, x => x.Value),
                    Sizes = await _context.Sizes.ToListAsync(),
                    Colors = await _context.Colors.ToListAsync(),
                    Categories = await _context.Categories.ToListAsync()
                };

                return View("Index", productVM);
            }
            else
            {
                List<Product> products = await _context.ProductToSizes
                    .Include(x => x.Product).Where(e => e.SizeId == id).Select(e => e.Product)
                    .ToListAsync();

                ProductVM productVM = new ProductVM
                {
                    Products = products,
                    Settings = await _context.Settings.ToDictionaryAsync(x => x.Key, x => x.Value),
                    Sizes = await _context.Sizes.ToListAsync(),
                    Colors = await _context.Colors.ToListAsync(),
                    Categories = await _context.Categories.ToListAsync()
                };

                return View("Index", productVM);
            }
        }
    }
}
