using Back_End_Project.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.ViewComponents
{
    public class HomeBannerViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(List<HomeBanner> homeBanners)
        {
            return View(await Task.FromResult(homeBanners));
        }
    }
}
