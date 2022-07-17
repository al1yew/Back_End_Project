using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Models
{
    public class Blog
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string BlogTitle { get; set; }

        [StringLength(255)]
        public string BlogImage { get; set; }

        public bool IsRecent { get; set; }

        public Nullable<DateTime> PublishDate { get; set; }

        public string UpperText { get; set; }

        public string StrongText { get; set; }

        public string BottomText { get; set; }

        public int CommentsCount { get; set; }


        //photos
        [NotMapped]
        public IFormFile BlogPhoto { get; set; }



        //relations of blog
        public BlogTag BlogTag { get; set; }
        public int BlogTagId { get; set; }
        public BlogCategory BlogCategory { get; set; }
        public int BlogCategoryId { get; set; }
        public BlogAuthor BlogAuthor { get; set; }
        public int BlogAuthorId { get; set; }


        //relations with blog
        public List<BlogComment> BlogComments { get; set; }


        //crud for Manage Area
        public Nullable<DateTime> CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public bool IsUpdated { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
    }
}
