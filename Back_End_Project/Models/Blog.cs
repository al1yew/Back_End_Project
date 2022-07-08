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
        //blog props - blog yazan yegin ki admindi cunki o admin panelden yazir, sonra onun burda relationun gurmag, AppUser ID ile
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


        //author
        [StringLength(255)]
        public string AuthorImage { get; set; }

        [StringLength(255)]
        public string AuthorName { get; set; }

        [StringLength(255)]
        public string AuthorPosition { get; set; }

        [StringLength(1024)]
        public string FacebookUrl { get; set; }

        [StringLength(1024)]
        public string TwitterUrl { get; set; }

        [StringLength(1024)]
        public string LinkedInUrl { get; set; }

        [StringLength(1024)]
        public string PinterestUrl { get; set; }


        //photos
        [NotMapped]
        public IFormFile BlogPhoto { get; set; }
        [NotMapped]
        public IFormFile AuthorPhoto { get; set; }


        //relations of blog
        public BlogTag BlogTag { get; set; }
        public int BlogTagId { get; set; }
        public BlogCategory BlogCategory { get; set; }
        public int BlogCategoryId { get; set; }

        //relations with blog
        public List<BlogComment> BlogComments { get; set; }

    }
}
