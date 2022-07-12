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

//proverit sortirovku v indekse vse do edinogo
//sortby search v custom js ya nemnogo zamenil, v footere popravil brands i categories, dal im asp-route

//sprosit u neqo pagination i viewbagi

namespace Back_End_Project.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? sortby, int? categoryId, int? brandId, string searchValue, int? colorId, int? sizeId, int sortbycount, int page = 1)
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
            //ViewBag.Select = 5;
            #endregion

            if (categoryId != null)
            {
                products = _context.Products
                    .Where(p => p.CategoryId == categoryId);

                ViewBag.CategoriesForProductsPage = products;
            }

            if (brandId != null)
            {
                products = _context.Products
                    .Where(p => p.BrandId == brandId);

                ViewBag.BrandsForProductsPage = products;
            }

            if (searchValue != null)
            {
                products = _context.Products
                .Where(p => p.Name.ToLower().Contains(searchValue.ToLower()) ||
                p.Brand.Name.ToLower().Contains(searchValue.ToLower()) ||
                p.Category.Name.ToLower().Contains(searchValue.ToLower()) ||
                p.Description.ToLower().Contains(searchValue.ToLower()) ||
                p.FirstText.ToLower().Contains(searchValue.ToLower()) ||
                p.SecondText.ToLower().Contains(searchValue.ToLower()));

                ViewBag.HeaderSearchForProductsPage = products;
            }

            if (colorId != null)
            {
                products = _context.ProductToColors
                   .Include(x => x.Product).Where(e => e.ColorId == colorId).Select(e => e.Product);

                ViewBag.ColorForProductsPage = products;
            }

            if (sizeId != null)
            {
                products = _context.ProductToSizes
                    .Include(x => x.Product).Where(e => e.SizeId == sizeId).Select(e => e.Product);

                ViewBag.SizeForProductsPage = products;
            }

            ProductVM productVM = new ProductVM
            {
                Products = PaginationList<Product>.Create(products, page, 5),
                Settings = await _context.Settings.ToDictionaryAsync(x => x.Key, x => x.Value),
                Sizes = await _context.Sizes.Include(c => c.ProductToSizes).ThenInclude(pc => pc.Product).ToListAsync(),
                Colors = await _context.Colors.Include(c => c.ProductToColors).ThenInclude(pc => pc.Product).ToListAsync(),
                Categories = await _context.Categories.Include(c => c.Products).ToListAsync()
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
    }
}
