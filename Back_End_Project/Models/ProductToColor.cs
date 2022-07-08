using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Models
{
    public class ProductToColor
    {
        public int Id { get; set; }


        //relations product and color models
        public Product Product { get; set; }
        public int ProductId { get; set; }

        public Color Color { get; set; }
        public int ColorId { get; set; }
    }
}
