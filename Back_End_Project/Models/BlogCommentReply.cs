using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Models
{
    public class BlogCommentReply
    {
        public int Id { get; set; }

        public string AdminImage { get; set; }
        public string AdminName { get; set; }

        public string UserImage { get; set; }
        public string Name { get; set; }

        public Nullable<DateTime> ResponseTime { get; set; }
        public string ResponseText { get; set; }
        public bool IsChildComment { get; set; }
        //yoxlanish uchun
        public string AdminUserId { get; set; }
        public string AppUserId { get; set; }


        //relation with blogcomment -- one comment at blog can have many replies, who wrote it and to which blog
        public BlogComment BlogComment { get; set; }
        public int BlogCommentId { get; set; }
        public int BlogId { get; set; }
    }
}
