using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokApp.DAL;
using PustokApp.Models;
using PustokApp.ViewModels;

namespace PustokApp.Controllers
{
    [AutoValidateAntiforgeryToken]

    public class BookController : Controller
    {
        private readonly AppDbContext _context;

        public BookController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();


        }
        public IActionResult Detail(int id)

        {
            if (id <= 0) return BadRequest();

            Book book = _context.Books
                .Include(b => b.Genre)
                .Include(b => b.Images)
                .Include(b => b.Author)
                .Include(b => b.BookTags).ThenInclude(bt => bt.Tag)
                .FirstOrDefault(book => book.Id == id)


                ;
            if (book is null) return NotFound();

            List<Book> relatedBooks = _context.Books
                
                .Where(b => b.GenreId == book.GenreId && book.Id != b.Id).Include(x=>x.Images)
                .Include(b => b.Author)
                .ToList();

            BookVM bookVM =
                new BookVM
                {
                    Book = book,
                    RelatedBooks = relatedBooks

                };
            return View(bookVM);
        }
    }
}
