﻿using Back_End_Project.DAL;
using Back_End_Project.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.ViewComponents
{
    public class HomeSliderViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(List<HomeSlider> homeSliders)
        {
            return View(await Task.FromResult(homeSliders));
        }
    }
}
