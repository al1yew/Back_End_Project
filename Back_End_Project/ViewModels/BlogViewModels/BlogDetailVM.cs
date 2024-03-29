﻿using Back_End_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.ViewModels.BlogViewModels
{
    public class BlogDetailVM
    {
        public Blog Blog { get; set; }
        public List<Blog> Blogs { get; set; }
        public BlogCommentVM BlogCommentVM { get; set; }
        public CommentReplyVM CommentReplyVM { get; set; }
    }
}
