using Back_End_Project.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.DAL
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        //models for PRODUCT
        public DbSet<Product> Products { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Basket> Baskets { get; set; }


        //models for ORDER
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }


        //models for SETTING
        public DbSet<Setting> Settings { get; set; }


        //models for HOME
        public DbSet<HomeBrandSlider> HomeBrandSliders { get; set; }
        public DbSet<HomeSlider> HomeSliders { get; set; }
        public DbSet<HomeService> HomeServices { get; set; }
        public DbSet<HomeBanner> HomeBanners { get; set; }

        //models for BLOG
        public DbSet<Blog> Blogs { get; set; }


        //relations one to many PRODUCT
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductInformation> ProductInformations { get; set; }


        //relations one to many BLOG
        public DbSet<BlogComment> BlogComments { get; set; }
        public DbSet<BlogCategory> BlogCategories { get; set; }
        public DbSet<BlogTag> BlogTags { get; set; }
        public DbSet<BlogAuthor> BlogAuthors { get; set; }


        //relations many to many PRODUCT
        public DbSet<ProductToColor> ProductToColors { get; set; }
        public DbSet<ProductToSize> ProductToSizes { get; set; }

    }
}
