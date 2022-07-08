using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Models
{
    public class BlogCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //relations
        public List<Blog> Blogs { get; set; }
    }
}
