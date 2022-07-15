using Back_End_Project.DAL;
using Back_End_Project.Models;
using Back_End_Project.ViewModels;
using Back_End_Project.ViewModels.BlogViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Index(int? tagid, int? authorid, string searchvalue, int sortbycount, int? categoryId, int page = 1)
        {
            IQueryable<Blog> blogs = _context.Blogs
                .Include(b => b.BlogAuthor)
                .Include(b => b.BlogCategory)
                .Include(b => b.BlogComments);

            if (sortbycount <= 0)
            {
                sortbycount = 6;
            }

            if (categoryId != null)
            {
                blogs = blogs
                    .Where(p => p.BlogCategoryId == categoryId);
            }

            if (tagid != null)
            {
                blogs = blogs
                    .Where(p => p.BlogTagId == tagid);
            }

            if (authorid != null)
            {
                blogs = blogs
                    .Where(p => p.BlogAuthorId == authorid);
            }

            if (searchvalue != null)
            {
                blogs = _context.Blogs
             .Where(b => b.BlogTitle.ToLower().Contains(searchvalue.ToLower()) ||
             b.BlogAuthor.AuthorName.ToLower().Contains(searchvalue.ToLower()) ||
             b.BlogCategory.Name.ToLower().Contains(searchvalue.ToLower()) ||
             b.BlogTag.Name.ToLower().Contains(searchvalue.ToLower()) ||
             b.UpperText.ToLower().Contains(searchvalue.ToLower()) ||
             b.StrongText.ToLower().Contains(searchvalue.ToLower()) ||
             b.BottomText.ToLower().Contains(searchvalue.ToLower()));
            }

            BlogVM blogVM = new BlogVM
            {
                Blogs = PaginationList<Blog>.Create(blogs, page, sortbycount),
                BlogTags = await _context.BlogTags.ToListAsync(),
                BlogAuthors = await _context.BlogAuthors.ToListAsync(),
                BlogCategories = await _context.BlogCategories.ToListAsync(),
                RecentBlogs = await _context.Blogs.Where(b => b.IsRecent).ToListAsync()
            };

            ViewBag.SelectForBlogs = sortbycount;
            ViewBag.CategoriesForBlogsPage = categoryId;
            ViewBag.TagsForBlogsPage = tagid;
            ViewBag.SearchForBlogsPage = searchvalue;
            ViewBag.AuthorForBlogsPage = authorid;

            //asp-route-tagid="@ViewBag.TagsForBlogsPage" asp-route-authorid="@ViewBag.AuthorForBlogsPage" asp-route-searchvalue="@ViewBag.SearchForBlogsPage" asp-route-categoryId="@ViewBag.CategoriesForBlogsPage" asp-route-sortbycount="@ViewBag.SelectForBlogs"

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
    }
}
