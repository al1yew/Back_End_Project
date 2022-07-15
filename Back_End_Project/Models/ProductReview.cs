using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Models
{
    public class ProductReview
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [StringLength(2048)]
        public string ReviewText { get; set; }

        public string AppUserId { get; set; }

        public int Rating { get; set; }


        //relations
        public Product Product { get; set; }
        public int ProductId { get; set; }


        //for date
        public Nullable<DateTime> CreatedAt { get; set; }
    }
}
