using Back_End_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.ViewModels.ProductViewModels
{
    public class ProductVM
    {
        public PaginationList<Product> Products { get; set; }
        public IDictionary<string, string> Settings { get; set; }
        public List<Color> Colors { get; set; }
        public List<Size> Sizes { get; set; }
        public List<Category> Categories { get; set; }
    }
}
