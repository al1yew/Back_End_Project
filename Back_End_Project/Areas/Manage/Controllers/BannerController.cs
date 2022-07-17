using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Areas.Manage.Controllers
{
    public class BannerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
