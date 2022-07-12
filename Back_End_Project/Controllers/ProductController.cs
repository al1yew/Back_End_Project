using Back_End_Project.DAL;
using Back_End_Project.Models;
using Back_End_Project.ViewModels;
using Back_End_Project.ViewModels.ProductViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//v indekse nujno popravit (say), sdelat sliderrange, on doljen otprravlat v metod po buttonu, ix mojno vzat v js
//i fetchanut ix kak dva rezultata, maxprice i minprice, i ya doljen sortirovat ix s pomoshyu etix dvux cen
//koqda delayesh pagination, posle togo kak vibral kategoriyu, i perekluchayeshsa na vtoruyu stranicku, on pochemu to sbrasivayet 
//kategorii i dayet zanovo vse
//detail page xochu shto b s inputa value mojno bilo vpisivat rukoy
//v header defaulte v klasse basketelementscount nado vpisat kolvo productov v baskete
//product detail stroka 67 s viewbagami, ix iskat v basketcontrollere

//VSE METODI SORTBY UBRAT!!!! V FOREACH DAT ASP-ROUTE-CATEGORYID ILI ASP-ROUTE-COLORID ITD I PRINIMAT IX VSEH V INDEKSE if(null deyilse)
//gedsin mene getirsin queryni


namespace Back_End_Project.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? sortby, int sortbycount, int page = 1)
        {
            IQueryable<Product> products = _context.Products
            .Include(p => p.Category)
            .Include(p => p.ProductToColors).ThenInclude(p => p.Color)
            .Include(p => p.ProductToSizes).ThenInclude(p => p.Size)
            .AsQueryable();
            #region pitalsa sdelat sortby

            //if (sortby != null)
            //{
            //    if (sortby == 1)
            //    {
            //        products.ToList().Sort();
            //    }
            //    else if (sortby == 2)
            //    {
            //        products.ToList().Reverse();
            //    }
            //    else if (sortby == 3)
            //    {
            //        products.ToList().OrderByDescending(c => c.Price);
            //    }
            //    else if (sortby == 4)
            //    {
            //        products.ToList().Sort((a, b) => b.Price.CompareTo(a.Price));
            //    }
            //    else if (sortby == 5)
            //    {
            //        products.ToList().Sort((a, b) => a.Price.CompareTo(b.Price));
            //    }
            //}

            //if (sortbycount <= 0)
            //{
            //    sortbycount = 5;
            //}

            //ViewBag.Sortby = sortby;
            #endregion

            ViewBag.Select = 5;

            ProductVM productVM = new ProductVM
            {
                Products = PaginationList<Product>.Create(products, page, sortbycount),
                Settings = await _context.Settings.ToDictionaryAsync(x => x.Key, x => x.Value),
                Sizes = await _context.Sizes.ToListAsync(),
                Colors = await _context.Colors.ToListAsync(),
                Categories = await _context.Categories.Include(c=>c.Products).ToListAsync()
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

        public async Task<IActionResult> SortByCategory(int? id, int sortbycount, int page = 1)
        {
            if (sortbycount <= 0)
            {
                sortbycount = 5;
            }

            ViewBag.Sortby = sortbycount;

            if (id == null)
            {
                IQueryable<Product> products = _context.Products.AsQueryable();

                ProductVM productVM = new ProductVM
                {
                    Products = PaginationList<Product>.Create(products, page, sortbycount),
                    Settings = await _context.Settings.ToDictionaryAsync(x => x.Key, x => x.Value),
                    Sizes = await _context.Sizes.ToListAsync(),
                    Colors = await _context.Colors.ToListAsync(),
                    Categories = await _context.Categories.ToListAsync()
                };

                return View("Index", productVM);
            }
            else
            {
                IQueryable<Product> products = _context.Products
                    .Where(p => p.CategoryId == id)
                    .AsQueryable();

                ProductVM productVM = new ProductVM
                {
                    Products = PaginationList<Product>.Create(products, page, sortbycount),
                    Settings = await _context.Settings.ToDictionaryAsync(x => x.Key, x => x.Value),
                    Sizes = await _context.Sizes.ToListAsync(),
                    Colors = await _context.Colors.ToListAsync(),
                    Categories = await _context.Categories.ToListAsync()
                };

                return View("Index", productVM);
            }

        }

        public async Task<IActionResult> SortByBrand(int? id, int sortbycount, int page = 1)
        {
            if (sortbycount <= 0)
            {
                sortbycount = 5;
            }

            ViewBag.Sortby = sortbycount;

            if (id == null)
            {
                IQueryable<Product> products =  _context.Products.AsQueryable();

                ProductVM productVM = new ProductVM
                {
                    Products = PaginationList<Product>.Create(products, page, sortbycount),
                    Settings = await _context.Settings.ToDictionaryAsync(x => x.Key, x => x.Value),
                    Sizes = await _context.Sizes.ToListAsync(),
                    Colors = await _context.Colors.ToListAsync(),
                    Categories = await _context.Categories.ToListAsync()
                };

                return View("Index", productVM);
            }
            else
            {
                IQueryable<Product> products =  _context.Products
                    .Where(p => p.BrandId == id)
                    .AsQueryable();

                ProductVM productVM = new ProductVM
                {
                    Products = PaginationList<Product>.Create(products, page, sortbycount),
                    Settings = await _context.Settings.ToDictionaryAsync(x => x.Key, x => x.Value),
                    Sizes = await _context.Sizes.ToListAsync(),
                    Colors = await _context.Colors.ToListAsync(),
                    Categories = await _context.Categories.ToListAsync()
                };

                return View("Index", productVM);
            }
        }

        public async Task<IActionResult> SortByColor(int? id, int sortbycount, int page = 1)
        {
            if (sortbycount <= 0)
            {
                sortbycount = 5;
            }

            ViewBag.Sortby = sortbycount;

            if (id == null)
            {
                IQueryable<Product> products = _context.Products.AsQueryable();

                ProductVM productVM = new ProductVM
                {
                    Products = PaginationList<Product>.Create(products, page, sortbycount),
                    Settings = await _context.Settings.ToDictionaryAsync(x => x.Key, x => x.Value),
                    Sizes = await _context.Sizes.ToListAsync(),
                    Colors = await _context.Colors.ToListAsync(),
                    Categories = await _context.Categories.ToListAsync()
                };

                return View("Index", productVM);
            }
            else
            {
                IQueryable<Product> products =  _context.ProductToColors
                    .Include(x => x.Product).Where(e => e.ColorId == id).Select(e => e.Product)
                    .AsQueryable();

                ProductVM productVM = new ProductVM
                {
                    Products = PaginationList<Product>.Create(products, page, sortbycount),
                    Settings = await _context.Settings.ToDictionaryAsync(x => x.Key, x => x.Value),
                    Sizes = await _context.Sizes.ToListAsync(),
                    Colors = await _context.Colors.ToListAsync(),
                    Categories = await _context.Categories.ToListAsync()
                };

                return View("Index", productVM);
            }
        }

        public async Task<IActionResult> SortBySize(int? id, int sortbycount, int page = 1)
        {
            if (sortbycount <= 0)
            {
                sortbycount = 5;
            }

            ViewBag.Sortby = sortbycount;

            if (id == null)
            {
                IQueryable<Product> products = _context.Products.AsQueryable();

                ProductVM productVM = new ProductVM
                {
                    Products = PaginationList<Product>.Create(products, page, sortbycount),
                    Settings = await _context.Settings.ToDictionaryAsync(x => x.Key, x => x.Value),
                    Sizes = await _context.Sizes.ToListAsync(),
                    Colors = await _context.Colors.ToListAsync(),
                    Categories = await _context.Categories.ToListAsync()
                };

                return View("Index", productVM);
            }
            else
            {
                IQueryable<Product> products =  _context.ProductToSizes
                    .Include(x => x.Product).Where(e => e.SizeId == id).Select(e => e.Product)
                    .AsQueryable();

                ProductVM productVM = new ProductVM
                {
                    Products = PaginationList<Product>.Create(products, page, sortbycount),
                    Settings = await _context.Settings.ToDictionaryAsync(x => x.Key, x => x.Value),
                    Sizes = await _context.Sizes.ToListAsync(),
                    Colors = await _context.Colors.ToListAsync(),
                    Categories = await _context.Categories.ToListAsync()
                };

                return View("Index", productVM);
            }
        }

        public async Task<IActionResult> SortBySearch(string sortbysearch, int sortbycount, int page = 1)
        {
            if (sortbycount <= 0)
            {
                sortbycount = 5;
            }

            ViewBag.Sortby = sortbycount;

            if (sortbysearch == null)
            {
                return View("Index");
            }

            IQueryable<Product> products = _context.Products
                .Where(p => p.Name.ToLower().Contains(sortbysearch.ToLower()) ||
                p.Brand.Name.ToLower().Contains(sortbysearch.ToLower()) ||
                p.Category.Name.ToLower().Contains(sortbysearch.ToLower()) ||
                p.Description.ToLower().Contains(sortbysearch.ToLower()) ||
                p.FirstText.ToLower().Contains(sortbysearch.ToLower()) ||
                p.SecondText.ToLower().Contains(sortbysearch.ToLower()))
                .AsQueryable();

            ProductVM productVM = new ProductVM
            {
                Products = PaginationList<Product>.Create(products, page, sortbycount),
                Settings = await _context.Settings.ToDictionaryAsync(x => x.Key, x => x.Value),
                Sizes = await _context.Sizes.ToListAsync(),
                Colors = await _context.Colors.ToListAsync(),
                Categories = await _context.Categories.ToListAsync()
            };

            return View("Index", productVM);
        }
    }
}
