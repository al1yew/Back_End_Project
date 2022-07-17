using Back_End_Project.Areas.Manage.ViewModels.BlogViewModels;
using Back_End_Project.DAL;
using Back_End_Project.Extensions;
using Back_End_Project.Models;
using Back_End_Project.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//hele mene blogtag ve blogcategory crudu lazimdi
namespace Back_End_Project.Areas.Manage.Controllers
{
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
        public async Task<IActionResult> Create(BlogVM blogVM)
        {
            ViewBag.Tags = await _context.BlogTags.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Categories = await _context.BlogCategories.Where(c => !c.IsDeleted).ToListAsync();
            ViewBag.Authors = await _context.BlogAuthors.ToListAsync();

            if (!ModelState.IsValid) return View();

            if (!await _context.BlogTags.AnyAsync(b => !b.IsDeleted && b.Id != blogVM.BlogTagId))
            {
                ModelState.AddModelError("BlogTagId", "Select Tag!");
                return View();
            }

            if (!await _context.Categories.AnyAsync(c => !c.IsDeleted && c.Id != blogVM.BlogCategoryId))
            {
                ModelState.AddModelError("BlogCategoryId", "Select category");
                return View();
            }

            if (blogVM.BlogAuthorId != 0 && blogVM.AuthorName != null && blogVM.AuthorPosition !=null && blogVM.AuthorSurname != null && blogVM.LinkedInUrl != null && blogVM.AuthorPhoto != null)
            {
                ModelState.AddModelError("", "You cant select author and also fill inputs below! Choose one option to do!");
                return View();
            }

            if (blogVM.BlogPhoto != null)
            {
                if (!blogVM.BlogPhoto.CheckContentType("image/jpeg")
                    && !blogVM.BlogPhoto.CheckContentType("image/jpg")
                    && !blogVM.BlogPhoto.CheckContentType("image/png")
                    && !blogVM.BlogPhoto.CheckContentType("image/gif"))
                {
                    ModelState.AddModelError("BlogPhoto", "Main image must be image format");
                    return View();
                }

                if (blogVM.BlogPhoto.CheckFileLength(15000))
                {
                    ModelState.AddModelError("BlogPhoto", "Main image size must be at most 15MB");
                    return View();
                }

                blogVM.BlogImage = await blogVM.BlogPhoto.CreateAsync(_env, "assets", "img", "blog");
            }
            else
            {
                ModelState.AddModelError("BlogPhoto", "Image is required");
                return View();
            }

            if (blogVM.AuthorPhoto != null)
            {
                if (!blogVM.AuthorPhoto.CheckContentType("image/jpeg")
                    && !blogVM.AuthorPhoto.CheckContentType("image/jpg")
                    && !blogVM.AuthorPhoto.CheckContentType("image/png")
                    && !blogVM.AuthorPhoto.CheckContentType("image/gif"))
                {
                    ModelState.AddModelError("AuthorPhoto", "Main image must be image format");
                    return View();
                }

                if (blogVM.AuthorPhoto.CheckFileLength(15000))
                {
                    ModelState.AddModelError("AuthorPhoto", "Main image size must be at most 15MB");
                    return View();
                }

                blogVM.AuthorImage = await blogVM.AuthorPhoto.CreateAsync(_env, "assets", "img", "blog");
            }

            blogVM.BlogTitle = blogVM.BlogTitle.Trim();

            #region Recognizing Author as appUser

            //AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            //BlogAuthor blogAuthor = new BlogAuthor
            //{
            //    AuthorImage = blogVM.AuthorImage != null ? blogVM.AuthorImage : appUser.Image,
            //    AuthorName = blogVM.AuthorName != null && blogVM.AuthorSurname != null ? blogVM.AuthorName + " " + blogVM.AuthorSurname : appUser.Name + " " + appUser.SurName,
            //    AuthorPosition = blogVM.AuthorPosition != null ? blogVM.AuthorPosition : "Admin",
            //    LinkedInUrl = blogVM.LinkedInUrl != null ? blogVM.LinkedInUrl : null
            //};

            //await _context.BlogAuthors.AddAsync(blogAuthor);
            //await _context.SaveChangesAsync();
            //bu onun uchun du ki, form boshdusa, log olan useri author kimi yazdirsin, forma nese deyer daxil edilibse, onlari gotursun.
            //ammd men hele de select option qatdim deye, her shey qarishmasin deye, yaxhsisi odur ki iki secim olsun, ya hazir select optionnan
            //secilsin, ya da ozu yazsin forma. 
            #endregion

            BlogAuthor blogAuthor = new BlogAuthor
            {
                AuthorImage = blogVM.AuthorImage,
                AuthorName = blogVM.AuthorName + " " + blogVM.AuthorSurname,
                AuthorPosition = blogVM.AuthorPosition,
                LinkedInUrl = blogVM.LinkedInUrl
            };

            if (blogAuthor.AuthorImage != null && blogAuthor.AuthorName != null && blogAuthor.AuthorPosition != null && blogAuthor.LinkedInUrl != null)
            {
                await _context.BlogAuthors.AddAsync(blogAuthor);
                await _context.SaveChangesAsync();
            }

            //forma hecne yazmayibsa, onda selectin authorid-si, yazibsa, select option value 0 gelecek, formdaki yeni avtorun melumatlari gedecek DBya

            Blog blog = new Blog
            {
                BlogAuthorId = blogVM.BlogAuthorId == 0 ? blogAuthor.Id : blogVM.BlogAuthorId,
                BlogCategoryId = blogVM.BlogCategoryId,
                BlogTagId = blogVM.BlogTagId,
                BlogImage = blogVM.BlogImage,
                BlogTitle = blogVM.BlogTitle,
                IsRecent = blogVM.IsRecent,
                PublishDate = DateTime.UtcNow.AddHours(4),
                UpperText = blogVM.UpperText,
                StrongText = blogVM.StrongText,
                BottomText = blogVM.BottomText
            };

            blog.CreatedAt = DateTime.UtcNow.AddHours(4);
            await _context.Blogs.AddAsync(blog);

            await _context.SaveChangesAsync();

            TempData["success"] = "Blog Is Created!";

            return RedirectToAction("Index");
        }
    }
}
