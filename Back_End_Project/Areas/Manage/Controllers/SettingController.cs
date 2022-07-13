using Back_End_Project.Areas.Manage.ViewModels.SettingViewModels;
using Back_End_Project.DAL;
using Back_End_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Areas.Manage.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    [Area("manage")]
    public class SettingController : Controller
    {
        private readonly AppDbContext _context;

        public SettingController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            IDictionary<string, string> settings = await _context.Settings.ToDictionaryAsync(s => s.Key, s => s.Value);

            return View(settings);
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] SettingVM settingVM)
        {
            List<Setting> settings = await _context.Settings.ToListAsync();

            if (!settings.Any(x => x.Key == settingVM.Key)) return BadRequest();

            settings.FirstOrDefault(x => x.Key == settingVM.Key).Value = settingVM.Value;

            await _context.SaveChangesAsync();

            TempData["success"] = $"{settingVM.Key} is updated";

            return PartialView("_SettingIndexPartial", settings.ToDictionary(x => x.Key, x => x.Value));
        }
    }
}
