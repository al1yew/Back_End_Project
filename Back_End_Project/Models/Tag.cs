using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Models
{
    public class Tag
    {
        public int Id { get; set; }

        [StringLength(255)]
        [Required]
        public string TagName { get; set; }

        //relations of Tag
        public IEnumerable<ProductToTag> ProductToTags { get; set; }
    }
}
