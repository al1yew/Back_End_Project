using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public double Price { get; set; }

        [Column(TypeName = "money")]
        public double DiscountPrice { get; set; }

        [Column(TypeName = "money")]
        public double Tax { get; set; }

        public int ReviewCount { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(maximumLength: 1000)]
        [Required]
        public string Description { get; set; }

        [Range(0, int.MaxValue)]
        public int Count { get; set; }

        [StringLength(maximumLength: 255)]
        public string Image { get; set; }

        [StringLength(maximumLength: 2048)]
        public string FirstText { get; set; }

        [StringLength(maximumLength: 2048)]
        public string SecondText { get; set; }

        //public string ProductInfo1 { get; set; }
        //public string ProductInfo2 { get; set; }
        //public string ProductInfo3 { get; set; }
        //public string ProductInfo4 { get; set; }
        //public string ProductInfo5 { get; set; }
        //cant add productInformaiton, cant understand how to pass 5 inputs without properties to List ProductInformation


        //for main home page
        public bool IsAvailable { get; set; }
        public bool IsTopSeller { get; set; }
        public bool IsNew { get; set; }


        //relations with product many to many
        public IEnumerable<ProductToColor> ProductToColors { get; set; }
        public IEnumerable<ProductToSize> ProductToSizes { get; set; }
        public IEnumerable<Basket> Baskets { get; set; }
        //---------------------Baskets is Many to many between product and user, many users can have many products 


        //relations with product one to many
        public List<ProductImage> ProductImages { get; set; }
        public List<ProductInformation> ProductInformation { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }
        public List<ProductReview> ProductReviews { get; set; }


        //relations of product
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public Brand Brand { get; set; }
        public int BrandId { get; set; }



        //images
        [NotMapped]
        public IFormFile Photo { get; set; }
        [NotMapped]
        public IEnumerable<IFormFile> Files { get; set; }


        //crud for Manage Area
        public Nullable<DateTime> CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public bool IsUpdated { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
    }
}
