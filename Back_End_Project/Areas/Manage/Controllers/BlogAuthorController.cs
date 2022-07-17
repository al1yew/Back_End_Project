using Back_End_Project.DAL;
using Back_End_Project.Extensions;
using Back_End_Project.Helper;
using Back_End_Project.Models;
using Back_End_Project.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class BlogAuthorController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;

        public BlogAuthorController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index(int? status, int select, int page = 1)
        {
            IQueryable<BlogAuthor> query = _context.BlogAuthors;

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

            return View(PaginationList<BlogAuthor>.Create(query, page, select));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogAuthor blogAuthor)
        {
            if (!ModelState.IsValid) return View();

            if (await _context.BlogAuthors.AnyAsync(b => b.AuthorName.ToLower().Trim() == blogAuthor.AuthorName.ToLower().Trim() && !b.IsDeleted))
            {
                ModelState.AddModelError("AuthorName", $"{blogAuthor.AuthorName} already exists");
                return View();
            }

            if (blogAuthor.AuthorPhoto != null)
            {
                if (!blogAuthor.AuthorPhoto.CheckContentType("image/jpeg")
                    && !blogAuthor.AuthorPhoto.CheckContentType("image/jpg")
                    && !blogAuthor.AuthorPhoto.CheckContentType("image/png")
                    && !blogAuthor.AuthorPhoto.CheckContentType("image/gif"))
                {
                    ModelState.AddModelError("AuthorPhoto", "Image must be image format");
                    return View();
                }

                if (blogAuthor.AuthorPhoto.CheckFileLength(15000))
                {
                    ModelState.AddModelError("AuthorPhoto", "Image size must be at most 15MB");
                    return View();
                }

                blogAuthor.AuthorImage = await blogAuthor.AuthorPhoto.CreateAsync(_env, "assets", "img", "blog");
            }

            blogAuthor.CreatedAt = DateTime.UtcNow.AddHours(+4);

            await _context.BlogAuthors.AddAsync(blogAuthor);
            await _context.SaveChangesAsync();

            TempData["success"] = "Author Is Created";

            return RedirectToAction("Index");

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
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            BlogAuthor blogAuthor = await _context.BlogAuthors.FirstOrDefaultAsync(b => b.Id == id);

            if (blogAuthor == null) return NotFound();

            return View(blogAuthor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, BlogAuthor blogAuthor)
        {
            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();

            if (id != blogAuthor.Id) return BadRequest();

            BlogAuthor dbBlogAuthor = await _context.BlogAuthors.FirstOrDefaultAsync(b => b.Id == blogAuthor.Id);

            if (dbBlogAuthor == null) return NotFound();
            //bunu name + " " + surname etmek unudma
            if (await _context.BlogAuthors.AnyAsync(b => b.Id != blogAuthor.Id && !b.IsDeleted && b.AuthorName.ToLower().Trim() == blogAuthor.AuthorName.ToLower().Trim()))
            {
                ModelState.AddModelError("AuthorName", $"{blogAuthor.AuthorName} already exists");
                return View();
            }

            if (blogAuthor.AuthorPhoto != null)
            {
                if (!blogAuthor.AuthorPhoto.CheckContentType("image/jpeg")
                    && !blogAuthor.AuthorPhoto.CheckContentType("image/jpg")
                    && !blogAuthor.AuthorPhoto.CheckContentType("image/png")
                    && !blogAuthor.AuthorPhoto.CheckContentType("image/gif"))
                {
                    ModelState.AddModelError("AuthorPhoto", "Main image must be image format");
                    return View();
                }

                if (blogAuthor.AuthorPhoto.CheckFileLength(15000))
                {
                    ModelState.AddModelError("AuthorPhoto", "Main image size must be at most 15MB");
                    return View();
                }

                FileHelper.DeleteFile(_env, dbBlogAuthor.AuthorImage, "assets", "img", "blog");

                dbBlogAuthor.AuthorImage = await blogAuthor.AuthorPhoto.CreateAsync(_env, "assets", "img", "blog");
            }

            dbBlogAuthor.AuthorName = blogAuthor.AuthorName.Trim();
            dbBlogAuthor.AuthorPosition = blogAuthor.AuthorPosition.Trim();
            dbBlogAuthor.LinkedInUrl = blogAuthor.LinkedInUrl.Trim();
            dbBlogAuthor.IsUpdated = true;
            dbBlogAuthor.UpdatedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            TempData["success"] = "Author Is Updated";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id, int? status, int select, int page)
        {
            if (id == null) return BadRequest();

            BlogAuthor blogAuthor = await _context.BlogAuthors.FirstOrDefaultAsync(b => b.Id == id);

            if (blogAuthor == null) return NotFound();

            blogAuthor.IsDeleted = true;
            blogAuthor.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            IQueryable<BlogAuthor> query = _context.BlogAuthors;

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

            TempData["success"] = "Author is deleted";

            return PartialView("_BlogAuthorIndexPartial", PaginationList<BlogAuthor>.Create(query, page, select));
        }

        public async Task<IActionResult> Restore(int? id, int? status, int select, int page)
        {
            if (id == null) return BadRequest();

            BlogAuthor blogAuthor = await _context.BlogAuthors.FirstOrDefaultAsync(b => b.Id == id);

            if (blogAuthor == null) return NotFound();

            blogAuthor.IsDeleted = false;
            blogAuthor.DeletedAt = null;

            await _context.SaveChangesAsync();

            IQueryable<BlogAuthor> query = _context.BlogAuthors;

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

            TempData["success"] = "Author is restored";

            return PartialView("_BlogAuthorIndexPartial", PaginationList<BlogAuthor>.Create(query, page, select));
        }
    }
}
