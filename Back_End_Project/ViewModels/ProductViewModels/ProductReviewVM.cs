using Back_End_Project.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.ViewModels.ProductViewModels
{
    public class ProductReviewVM
    {
        [StringLength(255)]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [StringLength(2048)]
        public string ReviewText { get; set; }

        public int Rating { get; set; }

        public int ProductId { get; set; }

        public string AppUserId { get; set; }
    }
}
