using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Models
{
    public class Setting
    {
        public int Id { get; set; }
        [Required, StringLength(255)]
        public string Key { get; set; }
        [Required, StringLength(2048)]
        public string Value { get; set; }
    }
}
