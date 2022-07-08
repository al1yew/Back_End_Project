using Back_End_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.ViewModels.HomeViewModels
{
    public class HomeVM
    {
        public List<HomeBrandSlider> HomeBrandSliders { get; set; }
        public List<HomeSlider> HomeSliders { get; set; }
        public List<Product> Products { get; set; }
        public List<Product> IsTopSeller { get; set; }
        public List<HomeService> HomeServices { get; set; }
        public List<HomeBanner> HomeBanners { get; set; }
        public List<Blog> Blogs { get; set; }
    }
}
