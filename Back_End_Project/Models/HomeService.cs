using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Models
{
    public class HomeService
    {
        public int Id { get; set; }
        public string Image { get; set; }
        [StringLength(30)]
        public string Title { get; set; }
        [StringLength(30)]
        public string SubTitle { get; set; }
        public string Color { get; set; }

        //Photo is Image Prop
        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
