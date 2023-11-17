using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
        {
            List<Slide> sliders = _context.Slides.OrderBy((s) => s.Id).ToList();

            List<Feature> feature = _context.Features.ToList();

            List<Book> books = _context.Books.ToList();
            

            HomeVM allClassesInOne = new HomeVM
            {
                Slides = sliders,
                Features = feature
            };

            return View(allClassesInOne);
        }
    }
}
