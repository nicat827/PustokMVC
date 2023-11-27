using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokApp.Areas.PustokArea.ViewModels;
using PustokApp.DAL;
using PustokApp.Models;
using PustokApp.Utilities.Enums;
using PustokApp.Utilities.Extencions;

namespace PustokApp.Areas.PustokArea.Controllers
{
    [Area("PustokArea")]
    public class SlideController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SlideController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;

        }

        public async Task<IActionResult> Index()
        {
            List<Slide> slides = await _context.Slides.ToListAsync();
            return View(slides);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSlideVM slideVM)
        {
            if (!ModelState.IsValid) return View();
            

            if (!slideVM.Photo.CheckFileType(FileType.Image))
            {
                ModelState.AddModelError("Photo", "Please, make sure you uploaded photo!");
                return View();
            }

            if (slideVM.Photo.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("Photo", "Photo size must be less than 2MB!");
                return View();
            }

            Slide slide = new Slide
            {
                Title = slideVM.Title,
                Subtitle = slideVM.Subtitle,
                Description = slideVM.Description,
                Order = slideVM.Order

            };

            slide.ImageURL = await slideVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "image", "bg-images");
            await _context.Slides.AddAsync(slide);
            await _context.SaveChangesAsync();

            

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            Slide slide = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);
            if (slide is null) return NotFound();


            slide.ImageURL.DeleteFile(_env.WebRootPath, "assets", "image", "bg-images");

            _context.Slides.Remove(slide);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));




        }


        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();

            Slide slide = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);
            if (slide is null) return NotFound();
            UpdateSlideVM slideVM = new UpdateSlideVM
            {
                Title = slide.Title,
                Subtitle = slide.Subtitle,
                Description = slide.Description,
                Order = slide.Order,
                ImageUrl = slide.ImageURL

            };
            return View(slideVM);
        }

        [HttpPost]

        public async Task<IActionResult> Update(int id, UpdateSlideVM slideVM)
        {
            if (!ModelState.IsValid) return View(slideVM);

            Slide slide = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);
            if (slide == null) return NotFound();


            if (slideVM.Photo is not null)
            {
                if (!slideVM.Photo.CheckFileType(FileType.Image))
                {
                    ModelState.AddModelError("Photo", "Please, make sure, you uploaded a photo!");
                    return View(slideVM);
                }

                if (slideVM.Photo.Length > 1024 * 1024 * 2)
                {
                    ModelState.AddModelError("Photo", "Photo size bigger than max!");
                    return View(slideVM);
                }

                slide.ImageURL.DeleteFile(_env.WebRootPath, "uploads", "slide");
                slide.ImageURL = await slideVM.Photo.CreateFileAsync(_env.WebRootPath, "uploads", "slide");


            }

            slide.Order = slideVM.Order;
            slide.Subtitle = slideVM.Subtitle;
            slide.Title = slideVM.Title;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0) return BadRequest();
            Slide slide = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);
            if (slide is null) return NotFound();

            return View(slide);
        }

        
    }
}
