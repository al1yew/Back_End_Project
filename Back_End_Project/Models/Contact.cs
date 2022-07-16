using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        [Phone]
        public string Phone { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [StringLength(255)]
        public string Subject { get; set; }

        public string Message { get; set; }

        public string AppUserId { get; set; }
    }
}
