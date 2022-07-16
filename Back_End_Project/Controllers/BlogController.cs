using Back_End_Project.DAL;
using Back_End_Project.Models;
using Back_End_Project.ViewModels;
using Back_End_Project.ViewModels.BlogViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//pochemu to v detail kategorii i tagi prinosit tupo vse podrad
namespace Back_End_Project.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public BlogController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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

            //AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name && !u.IsAdmin);

            //BlogCommentVM blogCommentVM = new BlogCommentVM
            //{
            //    Email = appUser.Email,
            //    AuthorName = appUser.Name,
            //};

            CommentReplyVM commentReplyVM = new CommentReplyVM
            {
                BlogCommentReplies = await _context.BlogCommentReplies.Where(b => b.BlogComment.BlogId == id).ToListAsync(),
                BlogComments = await _context.BlogComments.Where(b => b.BlogId == id).ToListAsync(),
                BlogCommentVM = new BlogCommentVM()
            };

            BlogDetailVM blogVM = new BlogDetailVM
            {
                Blog = blog,
                Blogs = await _context.Blogs.Include(b => b.BlogCategory).Include(b => b.BlogTag).ToListAsync(),
                BlogCommentVM = new BlogCommentVM(),
                CommentReplyVM = commentReplyVM
            };

            ViewBag.BlogId = id;

            return View(blogVM);
        }

        [HttpPost]
        public async Task<IActionResult> AddBlogComment(BlogCommentVM blogCommentVM, int id)
        {
            if (!ModelState.IsValid) return Redirect($"http://localhost:15866/Blog/Detail/{id}");

            if (id == null && id <= 0) return NotFound();

            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name && !u.IsAdmin);

            BlogComment blogComment = new BlogComment
            {
                Author = appUser == null ? blogCommentVM.AuthorName : appUser.Name,
                CreatedAt = DateTime.UtcNow.AddHours(4),
                Comment = blogCommentVM.Comment,
                Email = appUser == null ? blogCommentVM.Email : appUser.Email,
                AuthorImage = appUser == null ? null : appUser.Image,
                BlogId = id,
                AppUserId = appUser == null ? null : appUser.Id,
                IsParentComment = true,
            };

            Blog blog = await _context.Blogs.FirstOrDefaultAsync(p => p.Id == id);

            blog.CommentsCount++;

            await _context.BlogComments.AddAsync(blogComment);
            await _context.SaveChangesAsync();

            return Redirect($"http://localhost:15866/Blog/Detail/{id}");
        }


        [HttpGet]
        public async Task<IActionResult> AddCommentReply(int? id, int blogId)
        {
            BlogComment blogComment = await _context.BlogComments.FirstOrDefaultAsync(x => x.Id == id);

            ViewBag.BlogCommentId = blogComment.Id;
            ViewBag.BlogId = blogId;

            return PartialView("_CommentReplyPartial", new BlogCommentVM());
        }
        //evvelce detail page acilanda comment inputlari acilir. Reply knopkasina basanda JS Comment inutunu yigishdirir, COMMENTREPLY
        //inputunu yapishdirir, cunki metodlar ayridi, sorgu gelir HTTPGET-e, partiali yapishdirir input yerine, inuta deyerler daxil edilir, ad email comment, 
        //sonra yenede reply basanda gelir HTTPPOST-a, artiq set edir. 
        //Calishib bunlari bir metodda yazmag lazimdi, amma elebil biliklerim o qeder de cox deyil deye nese catmir, ki bunu duz yazim.
        //Redirect meselesi deli etdi meni

        [HttpPost]
        public async Task<IActionResult> AddCommentReply(BlogCommentVM blogCommentVM, int id, int blogId)
        {
            if (!ModelState.IsValid) return Redirect($"http://localhost:15866/Blog/Detail/{id}");

            if (id == null && id <= 0) return NotFound();

            BlogComment blogComment = await _context.BlogComments.FirstOrDefaultAsync(bc => bc.Id == id);

            if (blogComment == null) return NotFound();

            Blog blog = await _context.Blogs.FirstOrDefaultAsync(p => p.Id == blogComment.BlogId);

            if (blog == null) return NotFound();

            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            BlogCommentReply blogCommentReply = new BlogCommentReply
            {
                AdminImage = appUser != null && appUser.IsAdmin ? appUser.Image : null,
                AdminName = appUser != null && appUser.IsAdmin ? appUser.Name : null,
                IsChildComment = true,
                UserImage = appUser != null && !appUser.IsAdmin ? appUser.Image : null,
                Name = appUser != null && !appUser.IsAdmin ? appUser.Name : blogCommentVM.AuthorName,
                ResponseTime = DateTime.UtcNow.AddHours(4),
                ResponseText = blogCommentVM.Comment,
                AdminUserId = appUser != null && appUser.IsAdmin ? appUser.Id : null,
                AppUserId = appUser != null && !appUser.IsAdmin ? appUser.Id : null,
                BlogCommentId = id
            };

            //cox gozel situaciyadan cixdim ternary ile :DDD bele gozel body olmayib hec

            blog.CommentsCount++;
            blogComment.HasResponse = true;

            await _context.BlogCommentReplies.AddAsync(blogCommentReply);
            await _context.SaveChangesAsync();

            return Redirect($"http://localhost:15866/Blog/Detail/{blogId}");
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
