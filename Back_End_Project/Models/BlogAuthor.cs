using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Models
{
    public class BlogAuthor
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string AuthorImage { get; set; }

        [StringLength(255)]
        public string AuthorName { get; set; }

        [StringLength(255)]
        public string AuthorPosition { get; set; }

        [StringLength(1024)]
        public string LinkedInUrl { get; set; }


        [NotMapped]
        public IFormFile AuthorPhoto { get; set; }


        //relations
        public List<Blog> Blogs { get; set; }


        //crud for Manage Area
        public Nullable<DateTime> CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public bool IsUpdated { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
    }
}
