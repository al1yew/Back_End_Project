using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Image { get; set; }

        public bool IsMain { get; set; }

        public Nullable<int> ParentId { get; set; }

        public Category Parent { get; set; }
        public IEnumerable<Category> Children { get; set; }


        //relations with category
        public IEnumerable<Product> Products { get; set; }


        //crud for Manage Area
        public Nullable<DateTime> CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public bool IsUpdated { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }


        //Category photo will be assigned to property string Image
        [NotMapped]
        public IFormFile File { get; set; }
    }
}
