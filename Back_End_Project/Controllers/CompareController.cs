using Back_End_Project.DAL;
using Back_End_Project.Models;
using Back_End_Project.ViewModels.CompareViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Controllers
{
    public class CompareController : Controller
    {
        private readonly AppDbContext _context;

        public CompareController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            string comparelist = HttpContext.Request.Cookies["comparelist"];

            List<CompareVM> compareVMs = null;

            if (!string.IsNullOrWhiteSpace(comparelist))
            {
                compareVMs = JsonConvert.DeserializeObject<List<CompareVM>>(comparelist);
            }
            else
            {
                compareVMs = new List<CompareVM>();
            }

            return View(await _getCompareListItems(compareVMs));
        }


        public async Task<IActionResult> AddToCompare(int? id)
        {
            if (id == null) return BadRequest();

            Product product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            string comparelist = HttpContext.Request.Cookies["comparelist"];

            List<CompareVM> compareVMs = null;

            if (!string.IsNullOrWhiteSpace(comparelist))
            {
                compareVMs = JsonConvert.DeserializeObject<List<CompareVM>>(comparelist);
            }
            else
            {
                compareVMs = new List<CompareVM>();
            }

            if (compareVMs.Count < 3)
            {
                if (compareVMs.Exists(w => w.ProductId == id))
                {
                    TempData["info"] = "This product is already in Compare list!";
                }
                else
                {
                    CompareVM compareVM = new CompareVM
                    {
                        ProductId = product.Id,
                        IsAvailable = product.IsAvailable,
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        Image = product.Image,
                        //ColorName = product.ProductToColors.FirstOrDefault(x => x.ColorId == id).Color.Name,
                        CategoryName = product.Category.Name,
                        CategoryId = product.CategoryId
                    };

                    compareVMs.Add(compareVM);
                }
            }
            else
            {
                return View();
            }

            comparelist = JsonConvert.SerializeObject(compareVMs);

            HttpContext.Response.Cookies.Append("comparelist", comparelist);

            return PartialView("_ComparePartial", await _getCompareListItems(compareVMs));
        }

        public async Task<IActionResult> DeleteFromCompare(int? id)
        {
            if (id == null) return BadRequest();

            if (!await _context.Products.AnyAsync(p => p.Id == id)) return NotFound();

            string comparelist = HttpContext.Request.Cookies["comparelist"];

            if (string.IsNullOrWhiteSpace(comparelist)) return BadRequest();

            List<CompareVM> compareVMs = JsonConvert.DeserializeObject<List<CompareVM>>(comparelist);

            CompareVM compareVM = compareVMs.Find(b => b.ProductId == id);

            if (compareVM == null) return NotFound();

            compareVMs.Remove(compareVM);

            TempData["info"] = "Product is deleted from CompareList!";

            comparelist = JsonConvert.SerializeObject(compareVMs);

            Response.Cookies.Append("comparelist", comparelist);

            return PartialView("_ComparePartial", await _getCompareListItems(compareVMs));
        }



        private async Task<List<CompareVM>> _getCompareListItems(List<CompareVM> compareVMs)
        {
            foreach (CompareVM item in compareVMs)
            {
                Product dbProduct = await _context.Products.Include(p => p.ProductToColors).FirstOrDefaultAsync(p => p.Id == item.ProductId);

                item.Name = dbProduct.Name;
                item.Price = dbProduct.DiscountPrice > 0 ? dbProduct.DiscountPrice : dbProduct.Price;
                item.Image = dbProduct.Image;
                item.IsAvailable = dbProduct.IsAvailable;
                //item.ColorName = dbProduct.ProductToColors.FirstOrDefault(x => x.ColorId == item.ColorId).Color.Name;
                item.Description = dbProduct.Description;
            }

            return compareVMs;
        }
    }
}
