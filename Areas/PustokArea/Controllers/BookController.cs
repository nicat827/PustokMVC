using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokApp.Areas.PustokArea.ViewModels;
using PustokApp.DAL;
using PustokApp.Models;
using PustokApp.ViewModels;

namespace PustokApp.Areas.PustokArea.Controllers
{
    [Area("PustokArea")]
    public class BookController : Controller
    {
        private readonly AppDbContext _context;

        public BookController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Book> books = await _context.Books
                .Include(b => b.Images)
                .ToListAsync();

            return View(books);
        }

        public  async Task<IActionResult> Create()
        {
            List<Author> authors = await _context.Authors.ToListAsync();

            CreateBookVM createBookVM = new CreateBookVM { Authors = authors };
            createBookVM.Genres = await _context.Genres.ToListAsync();

            return View(createBookVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookVM bookVM)
        {
            if (!ModelState.IsValid)
            {
                bookVM.Authors = await _context.Authors.ToListAsync();
                bookVM.Genres = await _context.Genres.ToListAsync();
                return View(bookVM);
            }
            bool isExist = await _context.Books.AnyAsync(b => b.AuthorId == bookVM.AuthorId);

            if (!isExist)
            {
                ModelState.AddModelError("AuthorId", "Author with this name not exist!");
                bookVM.Authors = await _context.Authors.ToListAsync();
                bookVM.Genres = await _context.Genres.ToListAsync();

                return View(bookVM);
            }

            bool isExistGenre = await _context.Books.AnyAsync(b => b.GenreId == bookVM.GenreId);

            if (!isExistGenre)
            {
                ModelState.AddModelError("GenreId", "Genre with this name not exist!");
                bookVM.Authors = await _context.Authors.ToListAsync();
                bookVM.Genres = await _context.Genres.ToListAsync();

                return View(bookVM);
            }

            Book book = new Book
            {
                Name = bookVM.Name,
                Description = bookVM.Description,
                PageCount = (int)bookVM.PageCount,
                CostPrice = (int)bookVM.CostPrice,
                Discount = (int)bookVM.Discount,
                SalePrice = (int) bookVM.SalePrice,
                GenreId = (int)bookVM.GenreId,
                AuthorId = (int)bookVM.AuthorId,
                

            };

            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
