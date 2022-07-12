using Back_End_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.ViewModels.CompareViewModels
{
    public class CompareVM
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public double Price { get; set; }
        public bool IsAvailable { get; set; }

        //public string ColorName { get; set; }
        //public int ColorId { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}
