using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Models
{
    public class Brand
    {
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 255)]
        public string Name { get; set; }


        //relations with brand
        public IEnumerable<Product> Products { get; set; }



        //crud for Manage Area
        public Nullable<DateTime> CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public bool IsUpdated { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
    }
}
