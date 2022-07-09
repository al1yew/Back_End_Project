using Back_End_Project.DAL;
using Back_End_Project.ViewModels.FooterViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        public FooterViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(IDictionary<string, string> settings)
        {
            FooterVM footerVM = new FooterVM
            {
                Settings = settings,
                Brands = _context.Brands.ToList(),
                Categories = _context.Categories.ToList()
            };

            return View(await Task.FromResult(footerVM));
        }
    }
}
