using Back_End_Project.Models;
using Back_End_Project.ViewModels.BasketViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Interfaces
{
    public interface ILayoutService
    {
        Task<IDictionary<string, string>> GetSetting();
        Task<List<BasketVM>> GetBasket();

    }
}
