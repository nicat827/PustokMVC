using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokApp.DAL;
using PustokApp.Models;

namespace PustokApp.Areas.PustokArea.Controllers
{
    [Area("PustokArea")]

    public class GenreController : Controller
    {
        private readonly AppDbContext _context;

        public GenreController(AppDbContext context)
        {
            _context = context;
        }

        public async  Task<IActionResult> Index()
        {
            List<Genre> genres = await _context.Genres
                .Include(g => g.Books)
                .ToListAsync();
            return View(genres);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Genre genre)
        {
            if (!ModelState.IsValid)
            {
                return View();

            }

            bool isExist = await _context.Genres.AnyAsync(g => g.Name == genre.Name);

            if (isExist)
            {
                ModelState.AddModelError("Name", "This genre already existed!");
                Console.WriteLine("44");
                return View();
            }


            return Json(genre);

            
        }
    }
}
