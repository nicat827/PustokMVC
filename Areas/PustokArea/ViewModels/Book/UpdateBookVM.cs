using PustokApp.Models;
using System.ComponentModel.DataAnnotations;

namespace PustokApp.Areas.PustokArea.ViewModels
{
    public class UpdateBookVM
    {
        public string Name { get; set; }
        [Required]

        public int? PageCount { get; set; }
        [Required]

        public decimal? CostPrice { get; set; }
        [Required]

        public decimal? Discount { get; set; }
        [Required]
        public decimal? SalePrice { get; set; }

        public string Description { get; set; }

        [Required]

        public int? GenreId { get; set; }

        [Required]
        public int? AuthorId { get; set; }

        public bool IsAviable { get; set; }

        public List<Genre>? Genres { get; set; }
        public List<Author>? Authors { get; set; }
    }
}
