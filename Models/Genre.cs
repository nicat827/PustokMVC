using System.ComponentModel.DataAnnotations;

namespace PustokApp.Models
{
    public class Genre
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Input cant be null!")]
        [MaxLength(25, ErrorMessage = "Length cant be longer than 25!")]

        public string Name { get; set; }

        public List<Book>? Books { get; set; }
    }
}
