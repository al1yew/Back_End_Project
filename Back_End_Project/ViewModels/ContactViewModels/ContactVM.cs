using Back_End_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.ViewModels.ContactViewModels
{
    public class ContactVM
    {
        public IDictionary<string, string> Settings { get; set; }
        public ContactUsVM ContactUsVM { get; set; }
    }
}
