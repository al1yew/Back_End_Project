using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Areas.Manage.ViewModels.AccountViewModels
{
    public class ResetPasswordVM
    {
        [Required]
        [StringLength(255)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [StringLength(255)]
        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
