using Back_End_Project.ViewModels.BasketViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.ViewComponents
{
    public class BasketViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(BasketVM basketVM)
        {
            return View(await Task.FromResult(basketVM));
        }
    }
}
