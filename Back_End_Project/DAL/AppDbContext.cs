using Back_End_Project.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        //models
        public DbSet<Product> Products { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }


        //relations one to many
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductDescription> ProductDescriptions { get; set; }
        public DbSet<ProductInformation> ProductInformations { get; set; }


        //relations many to many
        public DbSet<ProductToColor> ProductToColors { get; set; }
        public DbSet<ProductToSize> ProductToSizes { get; set; }
        public DbSet<ProductToTag> ProductToTags { get; set; }

    }
}
