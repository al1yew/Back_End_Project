using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Models
{
    public class HomeSlider
    {
        public int Id { get; set; }

        [Required, StringLength(maximumLength: 255)]
        public string Image { get; set; }

        [Required, StringLength(maximumLength: 1024)]
        public string MainTitle { get; set; }

        [Required, StringLength(maximumLength: 1024)]
        public string SubTitle { get; set; }

        [Required, StringLength(maximumLength: 2048)]
        public string Description { get; set; }

        [Required, StringLength(maximumLength: 1024)]
        public string RedirectUrl { get; set; }
    }
}
