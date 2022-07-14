using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Areas.Manage.ViewModels.UserViewModels
{
    public class CreateUserVM
    {
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string SurName { get; set; }
        [Required]
        [StringLength(255)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(255)]
        public string UserName { get; set; }
        [Phone]
        public string Phone { get; set; }
        [Required]
        [StringLength(255)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [StringLength(255)]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public bool IsAdmin { get; set; }
    }
}
