
using System.ComponentModel.DataAnnotations;

namespace PustokApp.ViewModels
{
    public class LoginUserVM
    {
        [Required]
        [MinLength(4, ErrorMessage = "Username or Email must have min 4 length")]
        [MaxLength(80, ErrorMessage = "Username or Email must have max 80 length")]
        public string UserNameOrEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool IsRemembered { get; set; }
    }
}
