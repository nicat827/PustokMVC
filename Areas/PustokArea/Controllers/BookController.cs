using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokApp.Areas.PustokArea.ViewModels;
using PustokApp.DAL;
using PustokApp.Models;
using PustokApp.Utilities.Enums;
using PustokApp.Utilities.Extencions;
using PustokApp.ViewModels;

namespace PustokApp.Areas.PustokArea.Controllers
{
    [Area("PustokArea")]
    public class BookController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BookController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
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

            if (!bookVM.MainPhoto.CheckFileType(FileType.Image))
            {
                ModelState.AddModelError("MainPhoto", "Please make sure, you uploaded a photo!");
                bookVM.Authors = await _context.Authors.ToListAsync();
                bookVM.Genres = await _context.Genres.ToListAsync();
                bookVM.Tags = await GetTags();
                return View(bookVM);
            }

            if (!bookVM.MainPhoto.CheckFileSize(400, FileSize.Kilobite))
            {
                ModelState.AddModelError("MainPhoto", "Photo size bigger than we expect (400kB) :(!");
                bookVM.Authors = await _context.Authors.ToListAsync();
                bookVM.Genres = await _context.Genres.ToListAsync();
                bookVM.Tags = await GetTags();
                return View(bookVM);
            }

            if (!bookVM.HoverPhoto.CheckFileType(FileType.Image))
            {
                ModelState.AddModelError("HoverPhoto", "Please make sure, you uploaded a photo!");
                bookVM.Authors = await _context.Authors.ToListAsync();
                bookVM.Genres = await _context.Genres.ToListAsync();
                bookVM.Tags = await GetTags();
                return View(bookVM);
            }

            if (!bookVM.HoverPhoto.CheckFileSize(400, FileSize.Kilobite))
            {
                ModelState.AddModelError("HoverPhoto", "Photo size bigger than we expect (400kB) :(!");
                bookVM.Authors = await _context.Authors.ToListAsync();
                bookVM.Genres = await _context.Genres.ToListAsync();
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

            if (bookVM.TagIds is not null)
            {
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
                BookTags = new List<BookTag>(),
                Images = new List<BookImage>()
                
            };

            book.Images.Add(new BookImage { IsPrimary = true, Image = await bookVM.MainPhoto.CreateFileAsync(_env.WebRootPath, "uploads", "book") });
            book.Images.Add(new BookImage { IsPrimary = false, Image = await bookVM.HoverPhoto.CreateFileAsync(_env.WebRootPath, "uploads", "book") });
            TempData["ErrorMessage"] = "";
            if (bookVM.OtherPhotos is not null)
            {
                foreach (IFormFile photo in bookVM.OtherPhotos)
                {
                    if (!photo.CheckFileType(FileType.Image))
                    {
                        TempData["ErrorMessage"] += $"<p>Photo with name {photo.FileName}  wasn't created, beacuse type is not valid!</p>";
                        continue;
                    }
                    if (!photo.CheckFileSize(400, FileSize.Kilobite))
                    {
                        TempData["ErrorMessage"] += $"<p>Photo with name {photo.FileName}  wasn't created, beacuse size bigger than 400Kb!</p>";
                        continue;
                    }

                    book.Images.Add(new BookImage { IsPrimary = null, Image = await photo.CreateFileAsync(_env.WebRootPath, "uploads", "book")});


                }
            }


            if (bookVM.TagIds is not null)
            {
                foreach (int tagId in bookVM.TagIds)
                {
                    book.BookTags.Add(new BookTag { TagId = tagId });
                }
            }
           
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();

            Book book = await _context.Books
                .Include(b => b.Images)
                .Include(b => b.BookTags)
                .FirstOrDefaultAsync(b => b.Id == id);

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
                BookImages = book.Images,
                AuthorId = book.AuthorId,
                TagIds = book.BookTags.Select(b => b.TagId).ToList(),
                Tags = await GetTags(),
                Genres = await GetGenres(),
                Authors = await GetAuthors(),
                


            };

            return View(bookVM);
            
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateBookVM bookVM)
        {
            Book existed = await _context.Books
              .Include(b => b.BookTags)
              .Include(b => b.Images)
              .FirstOrDefaultAsync(b => b.Id == id);

            if (!ModelState.IsValid)
            {
                bookVM.Authors = await GetAuthors();
                bookVM.Genres = await GetGenres();
                bookVM.Tags = await GetTags();
                bookVM.BookImages = existed.Images;
                return View(bookVM);
            }

          

            if (existed == null) return NotFound();



            if (bookVM.AuthorId != existed.AuthorId)
            {
                bool isValidAuthorId = await _context.Authors.AnyAsync(a => a.Id == bookVM.AuthorId);
                if (!isValidAuthorId)
                {
                    ModelState.AddModelError("AuthorId", "Author with this name doesnt exist!");
                    bookVM.Authors = await GetAuthors();
                    bookVM.Genres = await GetGenres();
                    bookVM.Tags = await GetTags();
                    return View(bookVM);
                }
                existed.AuthorId = (int)bookVM.AuthorId;

            }
            if (bookVM.GenreId != existed.GenreId)
            {
                bool isValidGenreId = await _context.Genres.AnyAsync(a => a.Id == bookVM.GenreId);
                if (!isValidGenreId)
                {
                    ModelState.AddModelError("GenreId", "Genre with this name doesnt exist!");
                    bookVM.Authors = await GetAuthors();
                    bookVM.Genres = await GetGenres();
                    bookVM.Tags = await GetTags();
                    return View(bookVM);
                }
                existed.AuthorId = (int)bookVM.AuthorId;

            }

            if (bookVM.TagIds is not null)
            {
                existed.BookTags.RemoveAll(bTag => !bookVM.TagIds.Exists(tagId => tagId == bTag.TagId));
                

                foreach (int tagId in bookVM.TagIds)
                {
                    if (!await _context.BookTags.AnyAsync(bt => bt.TagId == tagId))
                    {
                        ModelState.AddModelError("TagIds", "Tag with this name doesnt exist!");
                        bookVM.Authors = await GetAuthors();
                        bookVM.Genres = await GetGenres();
                        bookVM.Tags = await GetTags();
                        return View(bookVM);
                    }
                    

                    if (!existed.BookTags.Exists(bt => bt.TagId == tagId))
                    {
                        existed.BookTags.Add(new BookTag { TagId = tagId });
                    }
                }
            }
            else existed.BookTags = new List<BookTag>();
            
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
