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
        [Required]
        public string Name { get; set; }

        [Phone]
        [Required]
        public string Phone { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [StringLength(255)]
        [Required]
        public string Subject { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
