using Back_End_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.ViewModels.BlogViewModels
{
    public class BlogVM
    {
        public List<Blog> RecentBlogs { get; set; }
        public PaginationList<Blog> Blogs { get; set; }
        public List<BlogCategory> BlogCategories { get; set; }
        public List<BlogAuthor> BlogAuthors { get; set; }
        public List<BlogTag> BlogTags { get; set; }
    }
}
