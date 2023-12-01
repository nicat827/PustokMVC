using PustokApp.Models;

namespace PustokApp.ViewModels
{
    public class HomeVM
    {
        public List<Slide> Slides { get; set; }
        public List<Feature> Features { get; set; }
        public List<Book> Books { get; set; }

        public List<Book>? BooksOnDiscount { get; set; }
        public List<Book>? NewBooks { get; set; }
        public List<Book>? ExpenciveBooks { get; set; }
    }
}
