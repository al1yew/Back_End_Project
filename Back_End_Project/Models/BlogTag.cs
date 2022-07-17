using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Models
{
    public class BlogTag
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //relations
        public List<Blog> Blogs { get; set; }

        //crud for Manage Area
        public Nullable<DateTime> CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public bool IsUpdated { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
    }
}
