using Back_End_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.ViewModels.ProductViewModels
{
    public class ProductDetailVM
    {
        public Product Product { get; set; }
        public List<Product> Products { get; set; }
        public List<ProductReview> ProductReviews { get; set; }
        public ProductReviewVM ProductReviewVM { get; set; }
    }
}
