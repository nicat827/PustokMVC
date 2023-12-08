using Microsoft.AspNetCore.Identity;
using PustokApp.Utilities.Enums;

namespace PustokApp.Models
{
    public class AppUser : IdentityUser
    {

        public string Name { get; set; }

        public string Surname { get; set; }

        public Gender Gender { get; set; }

        public List<BasketItem> BasketItems {get; set;}


    }
}
