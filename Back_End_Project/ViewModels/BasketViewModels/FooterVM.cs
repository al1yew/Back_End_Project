using Back_End_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.ViewModels.BasketViewModels
{
    public class FooterVM
    {
        public List<Category> Categories { get; set; }
        public List<Brand> Brands { get; set; }
        public IDictionary<string, string> Settings { get; set; }
    }
}
