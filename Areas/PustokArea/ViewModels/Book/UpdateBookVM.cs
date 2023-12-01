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
        public List<int>? TagIds { get; set; }
        public List<int>? ImageIds { get; set; }

        public IFormFile? MainPhoto { get; set; }
        public IFormFile? HoverPhoto { get; set; }

        public List<IFormFile>? OtherPhotos { get; set; }

        public List<Genre>? Genres { get; set; }
        public List<Author>? Authors { get; set; }

        public List<BookImage>? BookImages { get; set; }

        public List<Tag>? Tags { get; set; }

    }
}
