using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokApp.DAL;
using PustokApp.Models;
using PustokApp.ViewModels;

namespace PustokApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Slide> sliders = _context.Slides.OrderBy((s) => s.Id).ToList();

            List<Feature> feature = _context.Features.ToList();

            List<Book> books = _context.Books.Include(x => x.Author).Include(x=> x.Genre).Include(x => x.Images).ToList();



            HomeVM allClassesInOne = new HomeVM
            {
                Slides = sliders,
                Features = feature,
                Books = books,
                BooksOnDiscount = await _context.Books.Take(5).Where(b => b.Discount > 0).Include(b => b.Images.Where(i => i.IsPrimary != null)).ToListAsync(),
                NewBooks = await _context.Books.Take(5).OrderByDescending(b => b.Id).Include(b => b.Images.Where(i => i.IsPrimary != null)).ToListAsync(),
                ExpenciveBooks = await _context.Books.Take(5).OrderByDescending(b => b.SalePrice).Include(b => b.Images.Where(i => i.IsPrimary != null)).ToListAsync(),
            };

            return View(allClassesInOne);
        }
    }
}
