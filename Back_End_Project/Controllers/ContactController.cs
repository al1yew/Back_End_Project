using Back_End_Project.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


// v forme v stranicke index nujno prinesti ima familie Usera, esli on est, esli ego net zastavit ego napisat

namespace Back_End_Project.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;
        public ContactController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Settings.ToDictionaryAsync(x => x.Key, x => x.Value));
        }
    }
}
