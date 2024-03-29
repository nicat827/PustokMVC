﻿using PustokApp.Models;
using System.ComponentModel.DataAnnotations;

namespace PustokApp.Areas.PustokArea.ViewModels
{
    public class CreateBookVM
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

        //images
        public IFormFile MainPhoto { get; set; }
        public IFormFile HoverPhoto { get; set; }

        public List<IFormFile>? OtherPhotos { get; set; }



        [Required]
        public int? AuthorId { get; set; }

        public List<int>? TagIds { get; set; }

        public List<Genre>? Genres { get; set; }
        public List<Author>? Authors { get; set; }


        public List<Tag>? Tags { get; set; }



        //public List<BookImage> Images { get; set; }

        //public List<BookTag> BookTags { get; set; }

    }
}
