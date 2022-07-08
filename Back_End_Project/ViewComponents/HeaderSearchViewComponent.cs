using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.ViewComponents
{
    public class HeaderSearchViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(IDictionary<string, string> settings)
        {
            return View(await Task.FromResult(settings));
        }
    }
}
