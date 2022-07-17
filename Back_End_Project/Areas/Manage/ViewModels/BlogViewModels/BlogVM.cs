using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Areas.Manage.ViewModels.BlogViewModels
{
    public class BlogVM
    {
        [StringLength(255)]
        public string BlogTitle { get; set; }
        public int BlogId { get; set; }

        public bool IsRecent { get; set; }

        public Nullable<DateTime> PublishDate { get; set; }

        public string UpperText { get; set; }

        public string StrongText { get; set; }

        public string BottomText { get; set; }

        public string BlogImage { get; set; }


        //author

        [StringLength(255)]
        public string AuthorName { get; set; }

        public string AuthorImage { get; set; }

        [StringLength(255)]
        public string AuthorSurname { get; set; }

        [StringLength(255)]
        public string AuthorPosition { get; set; }

        [StringLength(1024)]
        public string LinkedInUrl { get; set; }


        //relations
        public int BlogTagId { get; set; }
        public int BlogCategoryId { get; set; }
        public int BlogAuthorId { get; set; }



        public IFormFile AuthorPhoto { get; set; }
        public IFormFile BlogPhoto { get; set; }
    }
}
