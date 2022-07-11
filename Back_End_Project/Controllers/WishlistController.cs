﻿using Back_End_Project.DAL;
using Back_End_Project.Models;
using Back_End_Project.ViewModels.WishlistViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Controllers
{
    public class WishlistController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public WishlistController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            string wishlist = HttpContext.Request.Cookies["wishlist"];

            List<WishlistVM> wishlistVMs = null;

            if (!string.IsNullOrWhiteSpace(wishlist))
            {
                wishlistVMs = JsonConvert.DeserializeObject<List<WishlistVM>>(wishlist);
            }
            else
            {
                wishlistVMs = new List<WishlistVM>();
            }

            return View(await _getWishlistItems(wishlistVMs));
        }

        public async Task<IActionResult> AddToWishlist(int? id)
        {
            if (id == null) return BadRequest();

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            string wishlist = HttpContext.Request.Cookies["wishlist"];

            List<WishlistVM> wishlistVMs = null;

            if (!string.IsNullOrWhiteSpace(wishlist))
            {
                wishlistVMs = JsonConvert.DeserializeObject<List<WishlistVM>>(wishlist);
            }
            else
            {
                wishlistVMs = new List<WishlistVM>();
            }

            if (wishlistVMs.Exists(w => w.ProductId == id))
            {
                TempData["info"] = "This product is already in Wishlist!";
            }
            else
            {
                WishlistVM wishlistVM = new WishlistVM
                {
                    ProductId = product.Id,
                    IsAvailable = product.IsAvailable,
                    Name = product.Name
                };

                wishlistVMs.Add(wishlistVM);
            }

            if (User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.Users.Include(u => u.Wishlists).FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

                if (appUser.Wishlists != null && appUser.Wishlists.Count() > 0)
                {
                    Wishlist dbWishlist = appUser.Wishlists.FirstOrDefault(b => b.ProductId == id);

                    if (dbWishlist == null)
                    {
                        Wishlist newWishlist = new Wishlist
                        {
                            ProductId = (int)id,
                        };

                        appUser.Wishlists.Add(newWishlist);
                    }
                }
                else
                {
                    List<Wishlist> wishlists = new List<Wishlist>
                    {
                        new Wishlist{
                            ProductId = (int)id
                        }
                    };

                    appUser.Wishlists = wishlists;
                }

                await _context.SaveChangesAsync();
            }

            wishlist = JsonConvert.SerializeObject(wishlistVMs);

            HttpContext.Response.Cookies.Append("wishlist", wishlist);

            return View("Index", await _getWishlistItems(wishlistVMs));
        }

        public async Task<IActionResult> DeleteFromWishlist()
        {
            return View();
        }

        private async Task<List<WishlistVM>> _getWishlistItems(List<WishlistVM> wishlistVMs)
        {
            //if (User.Identity.IsAuthenticated)
            //{
            //    AppUser appUser = await _userManager.Users.Include(u => u.Baskets).FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            //}

            foreach (WishlistVM item in wishlistVMs)
            {
                Product dbProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);

                item.Name = dbProduct.Name;
                item.Price = dbProduct.DiscountPrice > 0 ? dbProduct.DiscountPrice : dbProduct.Price;
                item.Tax = dbProduct.Tax;
                item.Image = dbProduct.Image;
            }

            return wishlistVMs;
        }

    }
}
