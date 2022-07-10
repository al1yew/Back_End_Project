using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.ViewModels.AccountViewModels
{
    public class ProfileVM
    {
        [StringLength(255), Required]
        public string Name { get; set; }
        [StringLength(255), Required]
        public string SurName { get; set; }
        [StringLength(255), Required]
        [EmailAddress]
        public string Email { get; set; }
        [StringLength(255), Required]
        public string UserName { get; set; }
        [StringLength(255)]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [StringLength(255)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [StringLength(255)]
        [Compare(nameof(NewPassword))]
        [DataType(DataType.Password)]
        public string ConfirmPasword { get; set; }
    }
}
