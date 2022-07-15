using Back_End_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.ViewModels.BlogViewModels
{
    public class CommentReplyVM
    {
        public List<BlogComment> BlogComments { get; set; }
        public List<BlogCommentReply> BlogCommentReplies { get; set; }
        public BlogCommentVM BlogCommentVM { get; set; }
    }
}
