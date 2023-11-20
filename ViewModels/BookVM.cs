using PustokApp.Models;

namespace PustokApp.ViewModels
{
    public class BookVM
    {
        public Book Book { get; set; }

        public List<Book> RelatedBooks { get; set; }
    }
}
