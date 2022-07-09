using Back_End_Project.DAL;
using Back_End_Project.Models;
using Back_End_Project.ViewModels.BlogViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//v index sdelat poisk po kategorii, avtoru, tegu, i sam Poisk, a takje pagination, 
//v detail sdelat form, voprosi doljni idti v admin panel, ottuda nado umet otvechat, mojno skinut v partial 

namespace Back_End_Project.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        public BlogController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Blog> blogs = await _context.Blogs
                .Include(b => b.BlogCategory)
                .Include(b => b.BlogTag)
                .Include(b => b.BlogAuthor)
                .ToListAsync();

            return View(blogs);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            Blog blog = await _context.Blogs
                .Include(b => b.BlogTag)
                .Include(b => b.BlogCategory)
                .Include(b => b.BlogComments)
                .Include(b => b.BlogAuthor)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (blog == null) return NotFound();

            BlogVM blogVM = new BlogVM
            {
                Blog = blog,
                Blogs = await _context.Blogs
                .Include(b => b.BlogCategory)
                .Include(b => b.BlogTag)
                .ToListAsync()
            };

            return View(blogVM);
        }
    }
}
