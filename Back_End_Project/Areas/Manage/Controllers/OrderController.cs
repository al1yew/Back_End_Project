using Back_End_Project.DAL;
using Back_End_Project.Enums;
using Back_End_Project.Models;
using Back_End_Project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Areas.Manage.Controllers
{

    [Area("manage")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? ordertype, int select, int page = 1)
        {
            IQueryable<Order> query = _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Product);

            if (ordertype != null && ordertype > 0)
            {
                switch (ordertype)
                {
                    case 1:
                        query = query.Where(x => x.OrderStatus == OrderStatus.Pending);
                        break;
                    case 2:
                        query = query.Where(x => x.OrderStatus == OrderStatus.Accepted);
                        break;
                    case 3:
                        query = query.Where(x => x.OrderStatus == OrderStatus.Rejected);
                        break;
                    case 4:
                        query = query.Where(x => x.OrderStatus == OrderStatus.Rejected);
                        break;
                    case 5:
                        query = query.Where(x => x.OrderStatus == OrderStatus.Shipped);
                        break;
                    case 6:
                        query = query.Where(x => x.OrderStatus == OrderStatus.Shipped);
                        break;
                    default:
                        break;
                }
            }

            if (select <= 0)
            {
                select = 5;
            }

            ViewBag.Select = select;

            ViewBag.Type = ordertype;

            return View(PaginationList<Order>.Create(query, page, select));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            Order order = await _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Product).FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();

            return View(order);
        }


        [HttpPost]
        public async Task<IActionResult> Update(int? id, OrderStatus orderstatus, string Comment)
        {
            if (id == null) return BadRequest();

            Order order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();

            if (orderstatus != OrderStatus.Rejected)
            {
                order.OrderStatus = orderstatus;
            }

            order.Comment = Comment;

            await _context.SaveChangesAsync();

            return RedirectToAction("index");
        }
    }
}
