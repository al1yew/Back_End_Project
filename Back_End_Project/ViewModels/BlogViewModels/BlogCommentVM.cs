using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.ViewModels.BlogViewModels
{
    public class BlogCommentVM
    {
        [StringLength(255)]
        public string AuthorName { get; set; }

        [StringLength(2048)]
        public string Comment { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Url]
        public string Website { get; set; }

        public int BlogId { get; set; }
        public int BlogCommentId { get; set; }
    }
}
