namespace PustokApp.Models
{
    public class Book
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int PageCount { get; set; }

        public bool IsAviable { get; set; }

        public bool IsDeleted { get; set; }

        public decimal CostPrice { get; set; }

        public decimal Discount { get; set; }

        public decimal SalePrice { get; set; }

        public string Description { get; set; }

        public int GenreId { get; set; }

        public Genre Genre { get; set; }

        public int AuthorId { get; set; }

        public Author Author { get; set; }

        public List<BookImage> Images { get; set; }

        public List<BookTag> BookTags { get; set; }




    }
}
