using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Models
{
    public class ProductToSize
    {
        public int Id { get; set; }


        //relations product and size models
        public Product Product { get; set; }
        public int ProductId { get; set; }

        public Size Size { get; set; }
        public int SizeId { get; set; }
    }
}
