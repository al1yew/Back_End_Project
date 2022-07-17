using Back_End_Project.Areas.Manage.ViewModels.SearchViewModels;
using Back_End_Project.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SearchController : Controller
    {
        private readonly AppDbContext _context;
        public SearchController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Search(string search)
        {
            SearchVM searchVM = new SearchVM
            {
                Brands = await _context.Brands
                .Where(b => b.Id.ToString().Contains(search.ToLower()) ||
                b.Name.ToLower().Contains(search.ToLower().Trim()))
                .ToListAsync(),

                Categories = await _context.Categories
                    .Where(b => b.Id.ToString().Contains(search.ToLower()) ||
                    b.Name.ToLower().Contains(search.ToLower()))
                    .ToListAsync(),

                Orders = await _context.Orders
                .Where(o => o.Id.ToString().Contains(search.ToLower().Trim()) ||
                o.City.ToLower().Contains(search.ToLower().Trim()) ||
                o.Country.ToLower().Contains(search.ToLower().Trim()) ||
                o.CompanyName.ToLower().Contains(search.ToLower().Trim()) ||
                o.Email.ToLower().Contains(search.ToLower().Trim()) ||
                o.Name.ToLower().Contains(search.ToLower().Trim()) ||
                o.Phone.ToLower().Contains(search.ToLower().Trim()) ||
                o.ZipCode.ToLower().Contains(search.ToLower().Trim()) ||
                o.SurName.ToLower().Contains(search.ToLower().Trim()) ||
                o.State.ToLower().Contains(search.ToLower().Trim()))
                .Include(o => o.OrderItems)
                .ToListAsync(),

                Products = await _context.Products
                .Where(p => p.Name.ToLower().Contains(search.ToLower()) ||
                p.Brand.Name.ToLower().Contains(search.ToLower()) ||
                p.Category.Name.ToLower().Contains(search.ToLower()) ||
                p.Description.ToLower().Contains(search.ToLower()) ||
                p.FirstText.ToLower().Contains(search.ToLower()) ||
                p.SecondText.ToLower().Contains(search.ToLower()))
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .ToListAsync(),

                Users = await _context.Users
                .Where(u => u.Email.Contains(search.Trim().ToLower()) ||
                u.Id.ToLower().Contains(search.Trim().ToLower()) ||
                u.Name.ToLower().Contains(search.Trim().ToLower()) ||
                u.PhoneNumber.Contains(search.Trim().ToLower()) ||
                u.SurName.ToLower().Contains(search.Trim().ToLower()) ||
                u.UserName.ToLower().Contains(search.Trim().ToLower()))
                .ToListAsync(),

                Blogs = await _context.Blogs
                .Where(b => b.BlogTitle.ToLower().Contains(search.ToLower()) ||
                b.BlogAuthor.AuthorName.ToLower().Contains(search.ToLower()) ||
                b.BlogCategory.Name.ToLower().Contains(search.ToLower()) ||
                b.BlogTag.Name.ToLower().Contains(search.ToLower()) ||
                b.UpperText.ToLower().Contains(search.ToLower()) ||
                b.StrongText.ToLower().Contains(search.ToLower()) ||
                b.BottomText.ToLower().Contains(search.ToLower()))
                .Include(b => b.BlogAuthor)
                .ToListAsync()
            };

            //return Content("okfekodsjvnsiudkfbsehdfbhjd");
            return PartialView("_ManageSearchPartial", searchVM);
        }
    }
}
