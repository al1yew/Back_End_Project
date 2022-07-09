using Back_End_Project.DAL;
using Back_End_Project.Models;
using Back_End_Project.ViewModels.HomeViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Product> products = await _context.Products.ToListAsync();

            HomeVM homeVM = new HomeVM
            {
                HomeBrandSliders = await _context.HomeBrandSliders.ToListAsync(),
                HomeSliders = await _context.HomeSliders.ToListAsync(),
                Products = products,
                IsTopSeller = products.Where(p => p.IsTopSeller).ToList(),
                HomeServices = await _context.HomeServices.ToListAsync(),
                HomeBanners = await _context.HomeBanners.ToListAsync(),
                Blogs = await _context.Blogs.Include(b=>b.BlogAuthor).ToListAsync()
            };

            return View(homeVM);
        }
    }
}
