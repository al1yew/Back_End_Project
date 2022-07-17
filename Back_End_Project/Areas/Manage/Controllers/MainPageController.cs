using Back_End_Project.DAL;
using Back_End_Project.Extensions;
using Back_End_Project.Helper;
using Back_End_Project.Models;
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
    public class MainPageController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public MainPageController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Banner()
        {
            return View(await _context.HomeBanners.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> BannerUpdate(int? id)
        {
            if (id == null) return BadRequest();

            HomeBanner homeBanner = await _context.HomeBanners.FirstOrDefaultAsync(x => x.Id == id);

            if (homeBanner == null) return NotFound();

            return View(homeBanner);
        }

        [HttpPost]
        public async Task<IActionResult> BannerUpdate(int? id, HomeBanner homeBanner)
        {
            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();

            if (id != homeBanner.Id) return BadRequest();

            HomeBanner dbHomeBanner = await _context.HomeBanners.FirstOrDefaultAsync(c => c.Id == homeBanner.Id);

            if (dbHomeBanner == null) return NotFound();

            if (homeBanner.BannerPhoto != null)
            {
                if (!homeBanner.BannerPhoto.CheckContentType("image/jpeg")
                    && !homeBanner.BannerPhoto.CheckContentType("image/jpg")
                    && !homeBanner.BannerPhoto.CheckContentType("image/png")
                    && !homeBanner.BannerPhoto.CheckContentType("image/gif"))
                {
                    ModelState.AddModelError("BannerPhoto", "You can choose only Image format!");
                    return View();
                }

                if (homeBanner.BannerPhoto.CheckFileLength(15000))
                {
                    ModelState.AddModelError("BannerPhoto", "File must be 15MB at most!");
                    return View();
                }

                FileHelper.DeleteFile(_env, dbHomeBanner.Image, "assets", "img", "banner");

                dbHomeBanner.Image = await homeBanner.BannerPhoto.CreateAsync(_env, "assets", "img", "banner");
            }

            dbHomeBanner.Title = homeBanner.Title.Trim();
            dbHomeBanner.SubTitle = homeBanner.SubTitle.Trim();
            dbHomeBanner.RedirectUrl = homeBanner.RedirectUrl.Trim();

            await _context.SaveChangesAsync();

            return RedirectToAction("Banner");
        }

        public async Task<IActionResult> BrandSlider()
        {
            return View(await _context.HomeBrandSliders.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> BrandSliderUpdate(int? id)
        {
            if (id == null) return BadRequest();

            HomeBrandSlider homeBrandSlider = await _context.HomeBrandSliders.FirstOrDefaultAsync(x => x.Id == id);

            if (homeBrandSlider == null) return NotFound();

            return View(homeBrandSlider);
        }

        [HttpPost]
        public async Task<IActionResult> BrandSliderUpdate(int? id, HomeBrandSlider homeBrandSlider)
        {
            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();

            if (id != homeBrandSlider.Id) return BadRequest();

            HomeBrandSlider dbHomeBrandSlider = await _context.HomeBrandSliders.FirstOrDefaultAsync(c => c.Id == homeBrandSlider.Id);

            if (dbHomeBrandSlider == null) return NotFound();

            if (homeBrandSlider.Photo != null)
            {
                if (!homeBrandSlider.Photo.CheckContentType("image/jpeg")
                    && !homeBrandSlider.Photo.CheckContentType("image/jpg")
                    && !homeBrandSlider.Photo.CheckContentType("image/png")
                    && !homeBrandSlider.Photo.CheckContentType("image/gif"))
                {
                    ModelState.AddModelError("Photo", "You can choose only Image format!");
                    return View();
                }

                if (homeBrandSlider.Photo.CheckFileLength(15000))
                {
                    ModelState.AddModelError("Photo", "File must be 15MB at most!");
                    return View();
                }

                FileHelper.DeleteFile(_env, dbHomeBrandSlider.Image, "assets", "img", "brand");

                dbHomeBrandSlider.Image = await homeBrandSlider.Photo.CreateAsync(_env, "assets", "img", "brand");
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("BrandSlider");
        }

        public async Task<IActionResult> Slider()
        {
            return View(await _context.HomeSliders.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> SliderUpdate(int? id)
        {
            if (id == null) return BadRequest();

            HomeSlider homeSlider = await _context.HomeSliders.FirstOrDefaultAsync(x => x.Id == id);

            if (homeSlider == null) return NotFound();

            return View(homeSlider);
        }

        [HttpPost]
        public async Task<IActionResult> SliderUpdate(int? id, HomeSlider homeSlider)
        {
            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();

            if (id != homeSlider.Id) return BadRequest();

            HomeSlider dbHomeSlider = await _context.HomeSliders.FirstOrDefaultAsync(c => c.Id == homeSlider.Id);

            if (dbHomeSlider == null) return NotFound();

            if (homeSlider.Photo != null)
            {
                if (!homeSlider.Photo.CheckContentType("image/jpeg")
                    && !homeSlider.Photo.CheckContentType("image/jpg")
                    && !homeSlider.Photo.CheckContentType("image/png")
                    && !homeSlider.Photo.CheckContentType("image/gif"))
                {
                    ModelState.AddModelError("Photo", "You can choose only Image format!");
                    return View();
                }

                if (homeSlider.Photo.CheckFileLength(15000))
                {
                    ModelState.AddModelError("Photo", "File must be 15MB at most!");
                    return View();
                }

                FileHelper.DeleteFile(_env, dbHomeSlider.Image, "assets", "img", "slider");

                dbHomeSlider.Image = await homeSlider.Photo.CreateAsync(_env, "assets", "img", "slider");
            }

            dbHomeSlider.MainTitle = homeSlider.MainTitle.Trim();
            dbHomeSlider.Description = homeSlider.Description.Trim();
            dbHomeSlider.SubTitle = homeSlider.SubTitle.Trim();
            dbHomeSlider.RedirectUrl = homeSlider.RedirectUrl.Trim();

            await _context.SaveChangesAsync();

            return RedirectToAction("Slider");
        }






        public async Task<IActionResult> Service()
        {
            return View(await _context.HomeServices.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> ServiceUpdate(int? id)
        {
            if (id == null) return BadRequest();

            HomeService homeService = await _context.HomeServices.FirstOrDefaultAsync(x => x.Id == id);

            if (homeService == null) return NotFound();

            return View(homeService);
        }

        [HttpPost]
        public async Task<IActionResult> ServiceUpdate(int? id, HomeService homeService)
        {
            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();

            if (id != homeService.Id) return BadRequest();

            HomeService dbHomeService = await _context.HomeServices.FirstOrDefaultAsync(c => c.Id == homeService.Id);

            if (dbHomeService == null) return NotFound();

            if (homeService.Photo != null)
            {
                if (!homeService.Photo.CheckContentType("image/jpeg")
                    && !homeService.Photo.CheckContentType("image/jpg")
                    && !homeService.Photo.CheckContentType("image/png")
                    && !homeService.Photo.CheckContentType("image/gif"))
                {
                    ModelState.AddModelError("Photo", "You can choose only Image format!");
                    return View();
                }

                if (homeService.Photo.CheckFileLength(15000))
                {
                    ModelState.AddModelError("Photo", "File must be 15MB at most!");
                    return View();
                }

                FileHelper.DeleteFile(_env, dbHomeService.Image, "assets", "img", "icon");

                dbHomeService.Image = await homeService.Photo.CreateAsync(_env, "assets", "img", "icon");
            }

            dbHomeService.Title = homeService.Title.Trim();
            dbHomeService.SubTitle = homeService.SubTitle.Trim();
            dbHomeService.Color = homeService.Color.Trim();

            await _context.SaveChangesAsync();

            return RedirectToAction("Service");
        }

    }
}
