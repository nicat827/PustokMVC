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
                .Where(b => b.IsDeleted == false)
                .Include(b => b.Images)
                .Include(b => b.Genre)
                .ToListAsync();



            return View(books);
        }

        public  async Task<IActionResult> Create()
        {
            List<Author> authors = await _context.Authors.ToListAsync();

            CreateBookVM createBookVM = new CreateBookVM { Authors = authors };
            createBookVM.Genres = await _context.Genres.ToListAsync();
            createBookVM.Tags = await _context.Tags.ToListAsync();

            return View(createBookVM);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0) return BadRequest();

            Book book = await _context.Books
                .Include(b => b.Genre)
                .Include(b => b.Author)
                .Include(b => b.BookTags).ThenInclude(bt => bt.Tag)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (book is null) return NotFound();

            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookVM bookVM)
        {
            if (!ModelState.IsValid)
            {
                bookVM.Authors = await GetAuthors();
                bookVM.Genres = await GetGenres();
                bookVM.Tags = await GetTags();
                return View(bookVM);
            }

            bool isExistAuthor = await _context.Authors.AnyAsync(a => a.Id == bookVM.AuthorId);

            if (!isExistAuthor)
            {
                ModelState.AddModelError("AuthorId", "Author with this name not exist!");
                bookVM.Authors = await _context.Authors.ToListAsync();
                bookVM.Genres = await _context.Genres.ToListAsync();
                bookVM.Tags = await GetTags();
                return View(bookVM);
            }

            bool isExistGenre = await _context.Genres.AnyAsync(g => g.Id == bookVM.GenreId);

            if (!isExistGenre)
            {
                ModelState.AddModelError("GenreId", "Genre with this name not exist!");
                bookVM.Authors = await _context.Authors.ToListAsync();
                bookVM.Genres = await _context.Genres.ToListAsync();
                bookVM.Tags = await GetTags();
                return View(bookVM);
            }

            foreach (int id in bookVM.TagIds)
            {
                bool isExist = await _context.Tags.AnyAsync(t => t.Id == id);
                if (!isExist)
                {
                    ModelState.AddModelError("TagIds", "Tag with this name doesnt exist!");
                    bookVM.Authors = await _context.Authors.ToListAsync();
                    bookVM.Genres = await _context.Genres.ToListAsync();
                    bookVM.Tags = await GetTags();
                    return View(bookVM);
                }
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
                IsAviable =true,
                IsDeleted = false,
                BookTags = new List<BookTag>()
                
            };

            foreach (int tagId in  bookVM.TagIds)
            {
                book.BookTags.Add(new BookTag { TagId = tagId });
            }

           

            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();

            Book book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book is null) return NotFound();
            UpdateBookVM bookVM = new UpdateBookVM
            {
                Name = book.Name,
                CostPrice = book.CostPrice,
                SalePrice = book.SalePrice,
                Discount = book.Discount,
                IsAviable = book.IsAviable,
                Description = book.Description,
                PageCount = book.PageCount,
                GenreId = book.GenreId,
                AuthorId = book.AuthorId,
                Genres = await GetGenres(),
                Authors = await GetAuthors(),
                


            };

            return View(bookVM);
            
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateBookVM bookVM)
        {
            if (!ModelState.IsValid)
            {
                bookVM.Authors = await GetAuthors();
                bookVM.Genres = await GetGenres();
                return View(bookVM);
            }

            Book existed = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (existed == null) return NotFound();
            if (bookVM.AuthorId != existed.AuthorId)
            {
                bool isValidAuthorId = await _context.Authors.AnyAsync(a => a.Id == bookVM.AuthorId);
                if (!isValidAuthorId)
                {
                    ModelState.AddModelError("AuthorId", "Author with this name doesnt exist!");
                    bookVM.Authors = await GetAuthors();
                    bookVM.Genres = await GetGenres();
                    return View(bookVM);
                }
                existed.AuthorId = (int) bookVM.AuthorId;
                   
            }
            if (bookVM.GenreId != existed.GenreId)
            {
                bool isValidGenreId = await _context.Genres.AnyAsync(a => a.Id == bookVM.GenreId);
                if (!isValidGenreId)
                {
                    ModelState.AddModelError("GenreId", "Genre with this name doesnt exist!");
                    bookVM.Authors = await GetAuthors();
                    bookVM.Genres = await GetGenres();
                    return View(bookVM);
                }
                existed.AuthorId = (int)bookVM.AuthorId;

            }
            
            existed.Description = bookVM.Description;
            existed.PageCount = (int)bookVM.PageCount;
            existed.CostPrice = (int)bookVM.CostPrice;
            existed.Discount = (int)bookVM.Discount;
            existed.SalePrice = (int)bookVM.SalePrice;
            existed.GenreId = (int)bookVM.GenreId;
            existed.AuthorId = (int)bookVM.AuthorId;
            existed.IsAviable = bookVM.IsAviable;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            Book book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book is null) return NotFound();

            book.IsDeleted = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<List<Genre>> GetGenres()
        {
            return await _context.Genres.ToListAsync();
        }
        public async Task<List<Author>> GetAuthors()
        {
            return await _context.Authors.ToListAsync();
        }
        public async Task<List<Tag>> GetTags()
        {
            return await _context.Tags.ToListAsync();
        }
    }
}
