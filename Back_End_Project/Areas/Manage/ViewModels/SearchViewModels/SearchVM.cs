using Back_End_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Areas.Manage.ViewModels.SearchViewModels
{
    public class SearchVM
    {
        public List<Category> Categories { get; set; }
        public List<Brand> Brands { get; set; }
        public List<Product> Products { get; set; }
        public List<AppUser> Users { get; set; }
        public List<Order> Orders { get; set; }
        public List<Blog> Blogs { get; set; }
    }
}
