using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Models
{
    public class ProductImage
    {
        public int Id { get; set; }

        //one to many relation
        [Required]
        [StringLength(maximumLength: 1000)]
        public string Image { get; set; }

        public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}
