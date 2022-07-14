using Back_End_Project.DAL;
using Back_End_Project.Models;
using Back_End_Project.ViewModels.AccountViewModels;
using Back_End_Project.ViewModels.BasketViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//vezde proverat esli user is admin poslat ego nax
//u kajdogo appuser doljni bit svoi payment details i adress1, adress2 itd, kotorie budut v Profile html v menu adresi i v menu Payment
//on sam proverayet kto zaregan poetomu pofiq esli admin v logine, on vse ravno izza [authorize(roles = member)] 
//nelza zaregatsa esli takoy user uje est - eto nado sdelat v register metode vnizu
//eto est v admin panele znacit ya napisal hazir v usercontroller
//napisat update dla adminov, tipa lubogo usera mojno sdelat adminom po jelaniyu, tipa esli useru ne oxota zaxodit na manage itd, esli eto knsh pravilno i realno
namespace Back_End_Project.Controllers
{
    public class AccountController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext _context;

        public AccountController(RoleManager<IdentityRole> roleManager,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            AppDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult LoginRegister()
        {
            RegisterVM registerVM = new RegisterVM();
            LoginVM loginVM = new LoginVM();

            LoginRegisterVM loginRegisterVM = new LoginRegisterVM
            {
                RegisterVM = registerVM,
                LoginVM = loginVM
            };

            return View(loginRegisterVM);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View();
            //var area = ControllerContext.RouteData.DataTokens["area"];

            AppUser appUser = new AppUser
            {
                Name = registerVM.Name,
                SurName = registerVM.SurName,
                UserName = registerVM.UserName,
                Email = registerVM.Email
            };

            IdentityResult result = await _userManager.CreateAsync(appUser, registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View();
            }

            result = await _userManager.AddToRoleAsync(appUser, "Member");

            await _signInManager.SignInAsync(appUser, true);

            string basketCookie = HttpContext.Request.Cookies["basket"];

            if (!string.IsNullOrWhiteSpace(basketCookie))
            {
                List<BasketVM> BasketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(basketCookie);

                List<Basket> baskets = new List<Basket>();

                foreach (BasketVM BasketVM in BasketVMs)
                {
                    if (appUser.Baskets != null && appUser.Baskets.Count() > 0)
                    {
                        Basket existedBasket = appUser.Baskets.FirstOrDefault(b => b.ProductId != BasketVM.ProductId);

                        if (existedBasket == null)
                        {
                            Basket basket = new Basket
                            {
                                AppUserId = appUser.Id,
                                ProductId = BasketVM.ProductId,
                                Count = BasketVM.Count
                            };

                            baskets.Add(basket);
                        }
                        else
                        {
                            existedBasket.Count += BasketVM.Count;
                            BasketVM.Count = existedBasket.Count;
                        }
                    }
                    else
                    {
                        Basket basket = new Basket
                        {
                            AppUserId = appUser.Id,
                            ProductId = BasketVM.ProductId,
                            Count = BasketVM.Count
                        };

                        baskets.Add(basket);
                    }
                }

                basketCookie = JsonConvert.SerializeObject(BasketVMs);

                HttpContext.Response.Cookies.Append("basket", basketCookie);

                await _context.Baskets.AddRangeAsync(baskets);
                await _context.SaveChangesAsync();
            }
            else
            {
                if (appUser.Baskets != null && appUser.Baskets.Count() > 0)
                {
                    List<BasketVM> BasketVMs = new List<BasketVM>();

                    foreach (Basket basket in appUser.Baskets)
                    {
                        BasketVM BasketVM = new BasketVM
                        {
                            ProductId = basket.ProductId,
                            Count = basket.Count
                        };

                        BasketVMs.Add(BasketVM);
                    }

                    basketCookie = JsonConvert.SerializeObject(BasketVMs);

                    HttpContext.Response.Cookies.Append("basket", basketCookie);
                }
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View();

            AppUser appUser = await _userManager.Users.Include(u => u.Baskets).FirstOrDefaultAsync(u => u.NormalizedEmail == loginVM.Email.Trim().ToUpperInvariant() && !u.IsAdmin && !u.IsDeleted);

            if (appUser == null)
            {
                RegisterVM registerVM = new RegisterVM();

                LoginRegisterVM loginRegisterVM = new LoginRegisterVM
                {
                    RegisterVM = registerVM,
                    LoginVM = loginVM
                };

                ModelState.AddModelError("", "Email or Password is wrong!");

                return View("LoginRegister", loginRegisterVM);
            }

            if (appUser.IsAdmin)
            {
                RegisterVM registerVM = new RegisterVM();

                LoginRegisterVM loginRegisterVM = new LoginRegisterVM
                {
                    RegisterVM = registerVM,
                    LoginVM = loginVM
                };

                ModelState.AddModelError("", "Email or Password is wrong!");

                return View("LoginRegister", loginRegisterVM);
            }

            if (!await _userManager.CheckPasswordAsync(appUser, loginVM.Password))
            {
                RegisterVM registerVM = new RegisterVM();

                LoginRegisterVM loginRegisterVM = new LoginRegisterVM
                {
                    RegisterVM = registerVM,
                    LoginVM = loginVM
                };

                ModelState.AddModelError("", "Email or Password is wrong!");

                return View("LoginRegister", loginRegisterVM);
            }

            await _signInManager.SignInAsync(appUser, loginVM.RememberMe);

            string basketCookie = HttpContext.Request.Cookies["basket"];

            if (!string.IsNullOrWhiteSpace(basketCookie))
            {
                List<BasketVM> BasketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(basketCookie);

                List<Basket> baskets = new List<Basket>();

                foreach (BasketVM BasketVM in BasketVMs)
                {
                    if (appUser.Baskets != null && appUser.Baskets.Count() > 0)
                    {
                        Basket existedBasket = appUser.Baskets.FirstOrDefault(b => b.ProductId != BasketVM.ProductId);

                        if (existedBasket == null)
                        {
                            Basket basket = new Basket
                            {
                                AppUserId = appUser.Id,
                                ProductId = BasketVM.ProductId,
                                Count = BasketVM.Count
                            };

                            baskets.Add(basket);
                        }
                        else
                        {
                            existedBasket.Count += BasketVM.Count;
                            BasketVM.Count = existedBasket.Count;
                        }
                    }
                    else
                    {
                        Basket basket = new Basket
                        {
                            AppUserId = appUser.Id,
                            ProductId = BasketVM.ProductId,
                            Count = BasketVM.Count
                        };

                        baskets.Add(basket);
                    }
                }

                basketCookie = JsonConvert.SerializeObject(BasketVMs);

                HttpContext.Response.Cookies.Append("basket", basketCookie);

                await _context.Baskets.AddRangeAsync(baskets);
                await _context.SaveChangesAsync();
            }
            else
            {
                if (appUser.Baskets != null && appUser.Baskets.Count() > 0)
                {
                    List<BasketVM> BasketVMs = new List<BasketVM>();

                    foreach (Basket basket in appUser.Baskets)
                    {
                        BasketVM BasketVM = new BasketVM
                        {
                            ProductId = basket.ProductId,
                            Count = basket.Count
                        };

                        BasketVMs.Add(BasketVM);
                    }

                    basketCookie = JsonConvert.SerializeObject(BasketVMs);

                    HttpContext.Response.Cookies.Append("basket", basketCookie);
                }
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Profile()
        {
            AppUser appUser = await _userManager.Users
               .Include(u => u.Orders).ThenInclude(o => o.OrderItems).ThenInclude(oi => oi.Product)
               .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (appUser == null) return NotFound();

            ProfileVM profileVM = new ProfileVM
            {
                Name = appUser.Name,
                SurName = appUser.SurName,
                Email = appUser.Email,
                UserName = appUser.UserName
            };

            MemberVM memberVM = new MemberVM
            {
                ProfileVM = profileVM,
                Orders = appUser.Orders
            };

            return View(memberVM);
        }

        [Authorize(Roles = "Member")]
        [HttpPost]
        public async Task<IActionResult> Update(ProfileVM profileVM)
        {
            if (!ModelState.IsValid) return View("Profile", profileVM);

            AppUser dbAppUser = await _userManager.FindByNameAsync(User.Identity.Name);

            if (dbAppUser.NormalizedUserName != profileVM.UserName.Trim().ToUpperInvariant() ||
                dbAppUser.Name.ToUpperInvariant() != profileVM.Name.Trim().ToUpperInvariant() ||
                dbAppUser.SurName.ToUpperInvariant() != profileVM.SurName.Trim().ToUpperInvariant() ||
                dbAppUser.NormalizedEmail != profileVM.Email.Trim().ToUpperInvariant())
            {
                dbAppUser.Name = profileVM.Name;
                dbAppUser.SurName = profileVM.SurName;
                dbAppUser.Email = profileVM.Email;
                dbAppUser.UserName = profileVM.UserName;

                IdentityResult identityResult = await _userManager.UpdateAsync(dbAppUser);

                if (!identityResult.Succeeded)
                {
                    foreach (var item in identityResult.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }

                    return View("Profile", profileVM);
                }

                TempData["success"] = "Account is updated!";
            }

            if (profileVM.CurrentPassword != null && profileVM.NewPassword != null)
            {
                if (await _userManager.CheckPasswordAsync(dbAppUser, profileVM.CurrentPassword) && profileVM.CurrentPassword == profileVM.NewPassword)
                {
                    ModelState.AddModelError("", "New password cannot be as same as current!");
                    return View("Profile", profileVM);
                }

                IdentityResult identityResult = await _userManager.ChangePasswordAsync(dbAppUser, profileVM.CurrentPassword, profileVM.NewPassword);

                if (!identityResult.Succeeded)
                {
                    foreach (var item in identityResult.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }

                    return View("Profile", profileVM);
                }

                TempData["success"] = "Account is Updated";
            }

            return RedirectToAction("Profile");
        }












        #region Created Roles

        //public async Task<IActionResult> CreateRole()
        //{
        //    await _roleManager.CreateAsync(new IdentityRole { Name = "SuperAdmin" });
        //    await _roleManager.CreateAsync(new IdentityRole { Name = "Member" });
        //    await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });

        //    return Content("Salammmmm");
        //}

        #endregion

        #region Created Super Admin

        //public async Task<IActionResult> CreateSuperAdmin()
        //{
        //    AppUser appUser = new AppUser
        //    {
        //        Name = "Admin",
        //        SurName = "Admin",
        //        UserName = "Admin",
        //        Email = "Admin@admin"
        //    };

        //    appUser.IsAdmin = true;

        //    await _userManager.CreateAsync(appUser, "Admin@123");

        //    await _userManager.AddToRoleAsync(appUser, "SuperAdmin");

        //    return Content("Super admin est ");
        //}
        #endregion
    }

}
