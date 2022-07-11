using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.ViewModels.WishlistViewModels
{
    public class WishlistVM
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public double Tax { get; set; }
        public bool IsAvailable { get; set; }
    }
}
