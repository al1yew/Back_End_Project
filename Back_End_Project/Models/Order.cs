using Back_End_Project.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string SurName { get; set; }

        public double TotalPrice { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [StringLength(255)]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(255)]
        public string Address1 { get; set; }

        [StringLength(255)]
        public string Address2 { get; set; }

        [Required]
        [StringLength(255)]
        public string Country { get; set; }

        [Required]
        [StringLength(255)]
        public string City { get; set; }

        [StringLength(255)]
        public string State { get; set; }

        [Required]
        [StringLength(10)]
        public string ZipCode { get; set; }

        [StringLength(2048)]
        public string Comment { get; set; }


        //relations --------
        public OrderStatus OrderStatus { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }


        //crud
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> CreatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
    }

}
