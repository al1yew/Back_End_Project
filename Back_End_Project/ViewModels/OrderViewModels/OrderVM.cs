using Back_End_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.ViewModels.OrderViewModels
{
    public class OrderVM
    {
        public Order Order { get; set; }
        public List<Basket> Baskets { get; set; }
    }
}
