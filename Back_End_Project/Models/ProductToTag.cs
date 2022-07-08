using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Models
{
    public class ProductToTag
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public Tag Tag { get; set; }
        public int TagId { get; set; }
        public int ProductId { get; set; }
    }
}
