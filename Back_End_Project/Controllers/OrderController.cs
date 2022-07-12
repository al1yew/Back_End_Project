using Back_End_Project.DAL;
using Back_End_Project.Enums;
using Back_End_Project.Models;
using Back_End_Project.ViewModels.AccountViewModels;
using Back_End_Project.ViewModels.OrderViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//koqda adres zad pishet nado shto b proveral pravilno, zastalal pisat pravilno, esli shto daval error shto zapolni
namespace Back_End_Project.Controllers
{
    [Authorize(Roles = "Member")]
    public class OrderController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public OrderController(UserManager<AppUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);

            List<Basket> baskets = await _context.Baskets.Include(b => b.Product).Where(b => b.AppUserId == appUser.Id).ToListAsync();

            Order order = new Order
            {
                Name = appUser.Name,
                SurName = appUser.SurName,
                Email = appUser.Email
            };

            LoginVM loginVM = new LoginVM();

            OrderVM orderVM = new OrderVM
            {
                Order = order,
                Baskets = baskets,
                LoginVM = loginVM
            };

            return View(orderVM);
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(Order order)
        {
            AppUser appUser = await _userManager.Users.Include(u => u.Baskets).ThenInclude(b => b.Product).FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            List<OrderItem> orderItems = new List<OrderItem>();

            foreach (Basket basket in appUser.Baskets)
            {
                OrderItem orderItem = new OrderItem
                {
                    Price = basket.Product.DiscountPrice > 0 ? basket.Product.DiscountPrice : basket.Product.Price,
                    Count = basket.Count,
                    ProductId = basket.ProductId,
                    TotalPrice = basket.Product.DiscountPrice > 0 ? basket.Product.DiscountPrice * basket.Count : basket.Product.Price * basket.Count
                };

                orderItems.Add(orderItem);
            }

            order.OrderItems = orderItems;
            order.CreatedAt = DateTime.UtcNow.AddHours(4);
            order.AppUserId = appUser.Id;
            order.OrderStatus = OrderStatus.Pending;
            order.TotalPrice = orderItems.Sum(o => o.TotalPrice);

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }

}