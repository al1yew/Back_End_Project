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
using Back_End_Project.Extensions;
using Microsoft.AspNetCore.Hosting;
using Back_End_Project.Helper;
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
        private readonly IWebHostEnvironment _env;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(AppDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IWebHostEnvironment env, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _env = env;
            _roleManager = roleManager;
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
        public async Task<IActionResult> Create(CreateUserVM createUserVM)
        {
            if (!ModelState.IsValid) return View();

            if (createUserVM.Photo != null)
            {
                if (!createUserVM.Photo.CheckContentType("image/jpeg")
                    && !createUserVM.Photo.CheckContentType("image/jpg")
                    && !createUserVM.Photo.CheckContentType("image/png")
                    && !createUserVM.Photo.CheckContentType("image/gif"))
                {
                    ModelState.AddModelError("Photo", "You can choose only Image format!");
                    return View();
                }

                if (createUserVM.Photo.CheckFileLength(15000))
                {
                    ModelState.AddModelError("Photo", "File must be 15MB at most!");
                    return View();
                }

                createUserVM.Image = await createUserVM.Photo.CreateAsync(_env, "manage", "img", "users");
            }

            AppUser user = new AppUser
            {
                Name = createUserVM.Name,
                SurName = createUserVM.SurName,
                UserName = createUserVM.UserName,
                Email = createUserVM.Email,
                IsAdmin = createUserVM.IsAdmin,
                PhoneNumber = createUserVM.Phone,
                Image = createUserVM.Image
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

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(string? id)
        {
            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);

            UpdateUserVM updateUserVM = new UpdateUserVM
            {
                Name = appUser.Name,
                SurName = appUser.SurName,
                UserName = appUser.UserName,
                Email = appUser.Email,
                IsAdmin = appUser.IsAdmin,
                Phone = appUser.PhoneNumber,
                Image = appUser.Image,
                AppUserId = appUser.Id,
            };

            return View(updateUserVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateUserVM updateUserVM, string id)
        {
            if (!ModelState.IsValid) return View();

            AppUser dbAppUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (updateUserVM.Photo != null)
            {
                if (!updateUserVM.Photo.CheckContentType("image/jpeg")
                    && !updateUserVM.Photo.CheckContentType("image/jpg")
                    && !updateUserVM.Photo.CheckContentType("image/png")
                    && !updateUserVM.Photo.CheckContentType("image/gif"))
                {
                    ModelState.AddModelError("Photo", "You can choose only Image format!");
                    return View();
                }

                if (updateUserVM.Photo.CheckFileLength(15000))
                {
                    ModelState.AddModelError("Photo", "File must be 15MB at most!");
                    return View();
                }

                updateUserVM.Image = await updateUserVM.Photo.CreateAsync(_env, "manage", "img", "users");

                if (dbAppUser.Image != null)
                {
                    FileHelper.DeleteFile(_env, dbAppUser.Image, "manage", "img", "users");
                }

                dbAppUser.Image = updateUserVM.Image;

                IdentityResult identityResult1 = await _userManager.UpdateAsync(dbAppUser);

                if (!identityResult1.Succeeded)
                {
                    ModelState.AddModelError("", "Upload image in right way!");

                    return View("Update", updateUserVM);
                }

                TempData["success"] = "Account is updated!";
            }

            if (dbAppUser.NormalizedUserName != updateUserVM.UserName.Trim().ToUpperInvariant() ||
                dbAppUser.Name.ToUpperInvariant() != updateUserVM.Name.Trim().ToUpperInvariant() ||
                dbAppUser.SurName.ToUpperInvariant() != updateUserVM.SurName.Trim().ToUpperInvariant() ||
                dbAppUser.NormalizedEmail != updateUserVM.Email.Trim().ToUpperInvariant())
            {
                dbAppUser.Name = updateUserVM.Name;
                dbAppUser.SurName = updateUserVM.SurName;
                dbAppUser.Email = updateUserVM.Email;
                dbAppUser.UserName = updateUserVM.UserName;
                dbAppUser.IsAdmin = updateUserVM.IsAdmin;
                dbAppUser.PhoneNumber = updateUserVM.Phone;

                IdentityResult identityResult = await _userManager.UpdateAsync(dbAppUser);

                if (!identityResult.Succeeded)
                {
                    foreach (var item in identityResult.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }

                    return View("Profile", updateUserVM);
                }

                TempData["success"] = "Account is updated!";
            }

            if (updateUserVM.IsAdmin && !dbAppUser.IsAdmin)
            {
                await _userManager.RemoveFromRoleAsync(dbAppUser, "Member");
                await _userManager.AddToRoleAsync(dbAppUser, "Admin");
            }
            else if (!updateUserVM.IsAdmin && dbAppUser.IsAdmin)
            {
                await _userManager.RemoveFromRoleAsync(dbAppUser, "Admin");
                await _userManager.AddToRoleAsync(dbAppUser, "Member");
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteProfileImage(string id)
        {
            if (id == null) return BadRequest();

            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(p => p.Id == id);

            if (appUser == null) return NotFound();

            FileHelper.DeleteFile(_env, appUser.Image, "manage", "img", "users");

            appUser.Image = null;

            IdentityResult identityResult = await _userManager.UpdateAsync(appUser);

            CreateUserVM createUserVM = new CreateUserVM
            {
                Name = appUser.Name,
                SurName = appUser.SurName,
                UserName = appUser.UserName,
                Email = appUser.Email,
                IsAdmin = appUser.IsAdmin,
                Phone = appUser.PhoneNumber,
                Image = appUser.Image,
                AppUserId = appUser.Id
            };

            return Content("");
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
