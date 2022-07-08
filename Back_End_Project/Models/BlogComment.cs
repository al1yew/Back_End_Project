using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Models
{
    public class BlogComment
    {
        public int Id { get; set; }
        public string CommentAuthor { get; set; }
        public Nullable<DateTime> CommentDate { get; set; }
        public string CommentText { get; set; }

        //relations
        public Blog Blog { get; set; }
        public int BlogId { get; set; }

    }
}
