using Back_End_Project.DAL;
using Back_End_Project.Models;
using Back_End_Project.ViewModels.BlogViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//v index v categories vrode gde (say) shto to polucilos sprosit u ucitela
//v index sdelat poisk po kategorii, avtoru, tegu, knopke poiska, a takje pagination, 
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
            List<Blog> blogs = await _context.Blogs.ToListAsync();

            BlogVM blogVM = new BlogVM
            {
                Blogs = blogs,
                BlogTags = await _context.BlogTags.ToListAsync(),
                BlogAuthors = await _context.BlogAuthors.ToListAsync(),
                BlogCategories = await _context.BlogCategories.ToListAsync(),
                RecentBlogs = await _context.Blogs.Where(b => b.IsRecent).ToListAsync()
            };

            return View(blogVM);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            Blog blog = await _context.Blogs
                .Include(b => b.BlogTag)
                .Include(b => b.BlogCategory)
                .Include(b => b.BlogComments)
                .Include(b => b.BlogAuthor)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (blog == null) return NotFound();

            BlogDetailVM blogVM = new BlogDetailVM
            {
                Blog = blog,
                Blogs = await _context.Blogs
                .Include(b => b.BlogCategory)
                .Include(b => b.BlogTag)
                .ToListAsync()
            };

            return View(blogVM);
        }
        public async Task<IActionResult> BlogSearch(string blogsearch)
        {
            List<Blog> blogs = await _context.Blogs
                .Where(b => b.BlogTitle.ToLower().Contains(blogsearch.ToLower()) ||
                b.BlogAuthor.AuthorName.ToLower().Contains(blogsearch.ToLower()) ||
                b.BlogCategory.Name.ToLower().Contains(blogsearch.ToLower()) ||
                b.BlogTag.Name.ToLower().Contains(blogsearch.ToLower()) ||
                b.UpperText.ToLower().Contains(blogsearch.ToLower()) ||
                b.StrongText.ToLower().Contains(blogsearch.ToLower()) ||
                b.BottomText.ToLower().Contains(blogsearch.ToLower()))
                .Include(b => b.BlogAuthor)
                .ToListAsync();

            return PartialView("_SearchBlogPartial", blogs);
        }
        public async Task<IActionResult> SortByCategory(int? id)
        {
            if (id == null)
            {
                return View("Index");
            }
            else
            {
                List<Blog> blogs = await _context.Blogs
                .Where(b => b.BlogCategoryId == id)
                .ToListAsync();

                BlogVM blogVM = new BlogVM
                {
                    Blogs = blogs,
                    BlogTags = await _context.BlogTags.ToListAsync(),
                    BlogAuthors = await _context.BlogAuthors.ToListAsync(),
                    BlogCategories = await _context.BlogCategories.ToListAsync(),
                    RecentBlogs = await _context.Blogs.Where(b => b.IsRecent).ToListAsync()
                };

                return View("Index", blogVM);
            }
        }
        public async Task<IActionResult> SortByTag(int? id)
        {
            if (id == null)
            {
                return View("Index");
            }
            else
            {
                List<Blog> blogs = await _context.Blogs
                .Where(b => b.BlogTagId == id)
                .ToListAsync();

                BlogVM blogVM = new BlogVM
                {
                    Blogs = blogs,
                    BlogTags = await _context.BlogTags.ToListAsync(),
                    BlogAuthors = await _context.BlogAuthors.ToListAsync(),
                    BlogCategories = await _context.BlogCategories.ToListAsync(),
                    RecentBlogs = await _context.Blogs.Where(b => b.IsRecent).ToListAsync()
                };

                return View("Index", blogVM);
            }
        }
        public async Task<IActionResult> SortByAuthor(int? id)
        {
            if (id == null)
            {
                return View("Index");
            }
            else
            {
                List<Blog> blogs = await _context.Blogs
                .Where(b => b.BlogAuthorId == id)
                .ToListAsync();

                BlogVM blogVM = new BlogVM
                {
                    Blogs = blogs,
                    BlogTags = await _context.BlogTags.ToListAsync(),
                    BlogAuthors = await _context.BlogAuthors.ToListAsync(),
                    BlogCategories = await _context.BlogCategories.ToListAsync(),
                    RecentBlogs = await _context.Blogs.Where(b => b.IsRecent).ToListAsync()
                };

                return View("Index", blogVM);
            }
        }
        public async Task<IActionResult> SortBySearch(string sortbysearch)
        {
            if (sortbysearch == null)
            {
                return View("Index");
            }

            List<Blog> blogs = await _context.Blogs
            .Where(b => b.BlogTitle.ToLower().Contains(sortbysearch.ToLower()) ||
            b.BlogAuthor.AuthorName.ToLower().Contains(sortbysearch.ToLower()) ||
            b.BlogCategory.Name.ToLower().Contains(sortbysearch.ToLower()) ||
            b.BlogTag.Name.ToLower().Contains(sortbysearch.ToLower()) ||
            b.UpperText.ToLower().Contains(sortbysearch.ToLower()) ||
            b.StrongText.ToLower().Contains(sortbysearch.ToLower()) ||
            b.BottomText.ToLower().Contains(sortbysearch.ToLower()))
            .Include(b => b.BlogAuthor)
            .ToListAsync();

            BlogVM blogVM = new BlogVM
            {
                Blogs = blogs,
                BlogTags = await _context.BlogTags.ToListAsync(),
                BlogAuthors = await _context.BlogAuthors.ToListAsync(),
                BlogCategories = await _context.BlogCategories.ToListAsync(),
                RecentBlogs = await _context.Blogs.Where(b => b.IsRecent).ToListAsync()
            };

            return View("Index", blogVM);
        }
    }
}
