using Back_End_Project.ViewModels.BasketViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.ViewModels.HeaderSearchViewModels
{
    public class HeaderSearchVM
    {
        public IDictionary<string, string> Settings { get; set; }
        public List<BasketVM> BasketVMs { get; set; }
    }
}
