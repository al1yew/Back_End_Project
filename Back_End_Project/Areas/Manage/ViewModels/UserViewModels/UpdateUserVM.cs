using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Areas.Manage.ViewModels.UserViewModels
{
    public class UpdateUserVM
    {
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string SurName { get; set; }

        [StringLength(255)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(255)]
        public string UserName { get; set; }

        [Phone]
        public string Phone { get; set; }

        public bool IsAdmin { get; set; }

        public string AppUserId { get; set; }

        public IFormFile Photo { get; set; }
        public string Image { get; set; }
    }
}
