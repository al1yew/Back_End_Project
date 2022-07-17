using Back_End_Project.DAL;
using Back_End_Project.Extensions;
using Back_End_Project.Helper;
using Back_End_Project.Models;
using Back_End_Project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Areas.Manage.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    [Area("Manage")]
    public class BlogController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public BlogController(AppDbContext context, IWebHostEnvironment env, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _context = context;
            _env = env;
        }
        public IActionResult Index(int? status, int select, int page = 1)
        {
            IQueryable<Blog> query = _context.Blogs
                .Include(p => p.BlogCategory)
                .Include(p => p.BlogAuthor)
                .Include(p => p.BlogComments).ThenInclude(p => p.BlogCommentReplies)
                .Include(p => p.BlogTag);

            if (status != null && status > 0)
            {
                if (status == 1)
                {
                    query = query.Where(p => p.IsDeleted);
                }
                else if (status == 2)
                {
                    query = query.Where(p => !p.IsDeleted);
                }
            }

            if (select <= 0)
            {
                select = 5;
            }

            ViewBag.Select = select;

            ViewBag.Status = status;

            return View(PaginationList<Blog>.Create(query, page, select));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Tags = await _context.BlogTags.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Categories = await _context.BlogCategories.Where(c => !c.IsDeleted).ToListAsync();
            ViewBag.Authors = await _context.BlogAuthors.ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog blog)
        {
            ViewBag.Tags = await _context.BlogTags.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Categories = await _context.BlogCategories.Where(c => !c.IsDeleted).ToListAsync();
            ViewBag.Authors = await _context.BlogAuthors.ToListAsync();

            if (!ModelState.IsValid) return View();

            if (!await _context.BlogTags.AnyAsync(b => !b.IsDeleted && b.Id == blog.BlogTagId))
            {
                ModelState.AddModelError("BlogTagId", "Select Tag!");
                return View();
            }

            if (!await _context.BlogCategories.AnyAsync(c => !c.IsDeleted && c.Id == blog.BlogCategoryId))
            {
                ModelState.AddModelError("BlogCategoryId", "Select category");
                return View();
            }

            if (!await _context.BlogAuthors.AnyAsync(c => !c.IsDeleted && c.Id == blog.BlogAuthorId))
            {
                ModelState.AddModelError("BlogAuthorId", "Select Author");
                return View();
            }

            if (blog.BlogPhoto != null)
            {
                if (!blog.BlogPhoto.CheckContentType("image/jpeg")
                    && !blog.BlogPhoto.CheckContentType("image/jpg")
                    && !blog.BlogPhoto.CheckContentType("image/png")
                    && !blog.BlogPhoto.CheckContentType("image/gif"))
                {
                    ModelState.AddModelError("BlogPhoto", "Image must be image format");
                    return View();
                }

                if (blog.BlogPhoto.CheckFileLength(15000))
                {
                    ModelState.AddModelError("BlogPhoto", "Image size must be at most 15MB");
                    return View();
                }

                blog.BlogImage = await blog.BlogPhoto.CreateAsync(_env, "assets", "img", "blog");
            }
            else
            {
                ModelState.AddModelError("BlogPhoto", "Image is required");
                return View();
            }

            blog.BlogTitle = blog.BlogTitle.Trim();

            blog.CreatedAt = DateTime.UtcNow.AddHours(4);
            await _context.Blogs.AddAsync(blog);

            await _context.SaveChangesAsync();

            TempData["success"] = "Blog Is Created!";

            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            Blog blog = await _context.Blogs.Include(b => b.BlogAuthor).Include(b => b.BlogTag).Include(b => b.BlogCategory).FirstOrDefaultAsync(c => !c.IsDeleted && c.Id == id);

            if (blog == null) return NotFound();

            ViewBag.Tags = await _context.BlogTags.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Categories = await _context.BlogCategories.Where(c => !c.IsDeleted).ToListAsync();
            ViewBag.Authors = await _context.BlogAuthors.ToListAsync();

            return View(blog);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, Blog blog)
        {
            ViewBag.Tags = await _context.BlogTags.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Categories = await _context.BlogCategories.Where(c => !c.IsDeleted).ToListAsync();
            ViewBag.Authors = await _context.BlogAuthors.ToListAsync();

            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();

            Blog dbBlog = await _context.Blogs.FirstOrDefaultAsync(c => !c.IsDeleted && c.Id == id);

            if (dbBlog == null) return NotFound();

            if (blog.BlogPhoto != null)
            {
                if (!blog.BlogPhoto.CheckContentType("image/jpeg")
                    && !blog.BlogPhoto.CheckContentType("image/jpg")
                    && !blog.BlogPhoto.CheckContentType("image/png")
                    && !blog.BlogPhoto.CheckContentType("image/gif"))
                {
                    ModelState.AddModelError("BlogPhoto", "Image must be image format");
                    return View();
                }

                if (blog.BlogPhoto.CheckFileLength(15000))
                {
                    ModelState.AddModelError("BlogPhoto", "Image size must be at most 15MB");
                    return View();
                }

                if (dbBlog.BlogImage != null)
                {
                    FileHelper.DeleteFile(_env, dbBlog.BlogImage, "assets", "img", "blog");
                }

                dbBlog.BlogImage = await blog.BlogPhoto.CreateAsync(_env, "assets", "img", "blog");
            }

            dbBlog.BlogTitle = blog.BlogTitle.Trim();
            dbBlog.BlogAuthorId = blog.BlogAuthorId;
            dbBlog.BlogCategoryId = blog.BlogCategoryId;
            dbBlog.BlogTagId = blog.BlogTagId;
            dbBlog.BlogImage = blog.BlogImage;
            dbBlog.IsRecent = blog.IsRecent;
            dbBlog.IsUpdated = true;
            dbBlog.UpdatedAt = DateTime.UtcNow.AddHours(4);
            dbBlog.UpperText = blog.UpperText.Trim();
            dbBlog.StrongText = blog.StrongText.Trim();
            dbBlog.BottomText = blog.BottomText.Trim();

            await _context.SaveChangesAsync();

            TempData["success"] = "Blog Is Updated!";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id, int? status, int select, int page = 1)
        {
            if (id == null) return BadRequest();

            Blog blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);

            if (blog == null) return NotFound();

            blog.IsDeleted = true;
            blog.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            IQueryable<Blog> query = _context.Blogs;

            if (status != null && status > 0)
            {
                if (status == 1)
                {
                    query = query.Where(b => b.IsDeleted);
                }
                else if (status == 2)
                {
                    query = query.Where(b => !b.IsDeleted);
                }
            }

            if (select <= 0)
            {
                select = 5;
            }

            ViewBag.Select = select;

            ViewBag.Status = status;

            TempData["success"] = "Blog is deleted";

            return PartialView("_BlogIndexPartial", PaginationList<Blog>.Create(query, page, select));
        }

        public async Task<IActionResult> Restore(int? id, int? status, int select, int page = 1)
        {
            if (id == null) return BadRequest();

            Blog blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);

            if (blog == null) return NotFound();

            blog.IsDeleted = false;
            blog.DeletedAt = null;

            await _context.SaveChangesAsync();

            IQueryable<Blog> query = _context.Blogs;

            if (status != null && status > 0)
            {
                if (status == 1)
                {
                    query = query.Where(b => b.IsDeleted);
                }
                else if (status == 2)
                {
                    query = query.Where(b => !b.IsDeleted);
                }
            }

            if (select <= 0)
            {
                select = 5;
            }

            ViewBag.Select = select;

            ViewBag.Status = status;

            TempData["success"] = "Blog is restored";

            return PartialView("_BlogIndexPartial", PaginationList<Blog>.Create(query, page, select));
        }

        public async Task<IActionResult> Comments(int select, int page = 1)
        {
            IQueryable<BlogComment> blogComments = _context.BlogComments.Include(b => b.BlogCommentReplies).AsQueryable();

            if (select <= 0)
            {
                select = 5;
            }

            ViewBag.Select = select;

            return View(PaginationList<BlogComment>.Create(blogComments, page, select));
        }

        [HttpGet]
        public async Task<IActionResult> Reply(int? id)
        {
            ViewBag.CommentForReply = _context.BlogComments.FirstOrDefaultAsync(x => x.Id == id).Result.Comment;
            //birdene komente gore viewmodel yaratmagi sehv bilirem
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Reply(int? id, BlogCommentReply blogCommentReply)
        {
            if (id == null) return BadRequest();

            BlogComment dbBlogComment = await _context.BlogComments.FirstOrDefaultAsync(b => b.Id == id);

            if (dbBlogComment == null) return NotFound();

            dbBlogComment.HasResponse = true;

            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            BlogCommentReply bcReply = new BlogCommentReply
            {
                AdminImage = appUser.Image,
                AdminName = appUser.Name,
                AdminUserId = appUser.Id,
                ResponseTime = DateTime.UtcNow.AddHours(4),
                IsChildComment = true,
                AppUserId = null,
                ResponseText = blogCommentReply.ResponseText,
                BlogCommentId = dbBlogComment.Id
            };

            await _context.BlogCommentReplies.AddAsync(bcReply);

            await _context.SaveChangesAsync();

            TempData["success"] = "Comment Is Replied";

            return RedirectToAction("Comments");
        }
    }
}
