using Back_End_Project.DAL;
using Back_End_Project.Models;
using Back_End_Project.ViewModels.ContactViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ContactController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name && !u.IsAdmin);

            ContactVM contactVM = new ContactVM
            {
                Settings = await _context.Settings.ToDictionaryAsync(x => x.Key, x => x.Value),
                ContactUsVM = new ContactUsVM
                {
                    Email = appUser == null ? null : appUser.Email,
                    Name = appUser == null ? null : appUser.Name,
                    Phone = appUser == null ? null : appUser.PhoneNumber
                }
            };

            return View(contactVM);
        }


        [HttpPost]
        public async Task<IActionResult> Contact(ContactUsVM contactUsVM)
        {
            if (contactUsVM == null) return BadRequest();

            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name && !u.IsAdmin);

            Contact contact = new Contact
            {
                Name = contactUsVM.Name.Trim(),
                Phone = contactUsVM.Phone,
                Message = contactUsVM.Message.Trim(),
                Subject = contactUsVM.Subject.Trim(),
                Email = contactUsVM.Email,
            };

            await _context.Contacts.AddAsync(contact);
            await _context.SaveChangesAsync();

            return Content("We will call you as soon as possible!");
        }
    }
}
