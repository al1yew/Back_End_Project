using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Models
{
    public class ProductDescription
    {
        public int Id { get; set; }

        [StringLength(2048)]
        public int FirstText { get; set; }

        [StringLength(2048)]
        public int SecondText { get; set; }


        //relation one to many
        public Product Product { get; set; }
        public int ProductId { get; set; }

    }
}
