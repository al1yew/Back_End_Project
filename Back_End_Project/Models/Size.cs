using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Models
{
    public class Size
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        //relations
        public IEnumerable<ProductToSize> ProductToSizes { get; set; }

    }
}
