using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Models
{
    public class BlogComment
    {
        //for users
        public int Id { get; set; }
        [StringLength(255)]
        public string Author { get; set; }

        public Nullable<DateTime> CreatedAt { get; set; }

        [StringLength(2048)]
        public string Comment { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string AuthorImage { get; set; }

        [Url]
        public string Website { get; set; }


        //boolean
        public bool HasResponse { get; set; }
        public bool IsParentComment { get; set; }


        //relations
        public Blog Blog { get; set; }
        public int BlogId { get; set; }
        public string AppUserId { get; set; }
        public List<BlogCommentReply> BlogCommentReplies { get; set; }

    }
}
