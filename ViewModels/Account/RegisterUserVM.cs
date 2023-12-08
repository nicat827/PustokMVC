using PustokApp.Utilities.Enums;
using System.ComponentModel.DataAnnotations;

namespace PustokApp.ViewModels
{
    public class RegisterUserVM
    {
        [Required]
        [MinLength(3, ErrorMessage = "Name cant be lower than 3")]
        [MaxLength(25, ErrorMessage = "Name cant be longer than 25")]
        public string Name { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Surname cant be lower than 3")]
        [MaxLength(50, ErrorMessage = "Surname cant be longer than 50")]
        public string Surname { get; set; }
        [Required]
        [MinLength(4, ErrorMessage = "Username cant be lower than 4")]
        [MaxLength(25, ErrorMessage = "Username cant be longer than 25")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [MinLength(6)]
        [MaxLength(80)]
        public string Email { get; set; }


        [Required]
        [MinLength(8, ErrorMessage = "Password cant be lower than 8")]
        [MaxLength(50, ErrorMessage = "Password cant be longer than 50")]
        [DataType(DataType.Password)]

        public string Password { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "Password cant be lower than 8")]
        [MaxLength(50, ErrorMessage = "Password cant be longer than 50")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password mismatch!")]
        public string ConfirmPassword { get; set; }

        
        public Gender Gender { get; set; }


    }
}
