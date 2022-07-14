using Back_End_Project.DAL;
using Back_End_Project.Models;
using Back_End_Project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End_Project.Areas.Manage.ViewModels.AccountViewModels;
using Microsoft.EntityFrameworkCore;
using Back_End_Project.Areas.Manage.ViewModels.UserViewModels;
//useri admin etmek ve admini user etmek, ROLES deyishmelidi!!!! isadmin ile ish bitmir!
//navbarda users tablesi var, onu gostermemek uchun user.identity.isinrole true false qaytarir, eto nado proverit
namespace Back_End_Project.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "SuperAdmin")]
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public UserController(AppDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index(int? status, int select, int role, int page = 1)
        {
            IQueryable<AppUser> query = _userManager.Users.Where(u => u.UserName != User.Identity.Name);

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

            if (role != null && role > 0)
            {
                if (role == 1)
                {
                    query = query.Where(b => b.IsAdmin);
                }
                else if (role == 2)
                {
                    query = query.Where(b => !b.IsAdmin);
                }
            }

            ViewBag.Select = select;

            ViewBag.Status = status;

            ViewBag.Role = role;

            return View(PaginationList<AppUser>.Create(query, page, select));
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest();

            AppUser appUser = await _userManager.FindByIdAsync(id);

            if (appUser == null) return NotFound();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string id, ResetPasswordVM resetPasswordVM)
        {
            if (!ModelState.IsValid) return View();

            if (string.IsNullOrWhiteSpace(id)) return BadRequest();

            AppUser appUser = await _userManager.FindByIdAsync(id);

            if (appUser == null) return NotFound();

            if (await _userManager.CheckPasswordAsync(appUser, resetPasswordVM.Password)) return BadRequest();

            string token = await _userManager.GeneratePasswordResetTokenAsync(appUser);
            //string emailConfirm = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);

            IdentityResult identityResult = await _userManager.ResetPasswordAsync(appUser, token, resetPasswordVM.Password);
            //IdentityResult identityResult = await _userManager.ChangeEmailAsync(appUser, token, resetPasswordVM.Password);

            if (!identityResult.Succeeded)
            {
                foreach (var item in identityResult.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }

                return View();
            }
            //mojet bit ponadobitsa sdes toje polucit status i page v skobkax kak v Index metode
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserVM createUserVM, int role, int select, int page = 1)
        {
            if (!ModelState.IsValid) return View();

            AppUser dbAppUser = await _userManager.FindByEmailAsync(createUserVM.Email);
            List<AppUser> appUsers = await _context.Users.ToListAsync();

            if (dbAppUser != null && appUsers.Any(a => a.UserName == createUserVM.UserName))
            {
                ModelState.AddModelError("", "Admin exists");
                return View();
            }

            AppUser user = new AppUser
            {
                Name = createUserVM.Name,
                SurName = createUserVM.SurName,
                UserName = createUserVM.UserName,
                Email = createUserVM.Email,
                IsAdmin = createUserVM.IsAdmin
            };

            IdentityResult result = await _userManager.CreateAsync(user, createUserVM.Password);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View();
            }

            if (createUserVM.IsAdmin)
            {
                result = await _userManager.AddToRoleAsync(user, "Admin");
            }
            else
            {
                result = await _userManager.AddToRoleAsync(user, "Member");
            }

            IQueryable<AppUser> query = _userManager.Users.Where(u => u.UserName != User.Identity.Name);

            if (select <= 0)
            {
                select = 5;
            }

            ViewBag.Select = select;

            ViewBag.Role = role;

            return View("Index", PaginationList<AppUser>.Create(query, page, select));
        }

        public async Task<IActionResult> Delete(string? id, int select, int role, int page = 1)
        {
            AppUser appUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (appUser == null) return NotFound();

            appUser.IsDeleted = true;
            appUser.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            IQueryable<AppUser> query = _userManager.Users.Where(u => u.UserName != User.Identity.Name);

            if (select <= 0)
            {
                select = 5;
            }

            if (role != null && role > 0)
            {
                if (role == 1)
                {
                    query = query.Where(b => b.IsAdmin);
                }
                else if (role == 2)
                {
                    query = query.Where(b => !b.IsAdmin);
                }
            }

            ViewBag.Select = select;

            ViewBag.Role = role;

            return PartialView("_UserIndexPartial", PaginationList<AppUser>.Create(query, page, select));
        }

        public async Task<IActionResult> Restore(string? id, int select, int role, int page = 1)
        {
            AppUser appUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (appUser == null) return NotFound();

            appUser.IsDeleted = false;
            appUser.DeletedAt = null;

            await _context.SaveChangesAsync();

            IQueryable<AppUser> query = _userManager.Users.Where(u => u.UserName != User.Identity.Name);

            if (select <= 0)
            {
                select = 5;
            }

            if (role != null && role > 0)
            {
                if (role == 1)
                {
                    query = query.Where(b => b.IsAdmin);
                }
                else if (role == 2)
                {
                    query = query.Where(b => !b.IsAdmin);
                }
            }

            ViewBag.Select = select;

            return PartialView("_UserIndexPartial", PaginationList<AppUser>.Create(query, page, select));
        }
    }
}
