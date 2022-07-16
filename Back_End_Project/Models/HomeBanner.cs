using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Models
{
    public class HomeBanner
    {
        public int Id { get; set; }

        public string Image { get; set; }

        [StringLength(40)]
        public string Title { get; set; }

        [StringLength(40)]
        public string SubTitle { get; set; }
        public string RedirectUrl { get; set; }


        [NotMapped]
        public IFormFile BannerPhoto { get; set; }
    }
}
