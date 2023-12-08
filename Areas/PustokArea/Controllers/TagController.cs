using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokApp.DAL;
using PustokApp.Models;
using System.Data;

namespace PustokApp.Areas.PustokArea.Controllers
{
    [Area("PustokArea")]
    [Authorize(Roles = "Admin")]
    [AutoValidateAntiforgeryToken]
    public class TagController : Controller
    {
        private readonly AppDbContext _context;

        public TagController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Tag> Tags = await _context.Tags
                .Include(t => t.BookTags)
                .ToListAsync();
            return View(Tags);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Tag tag)
        {
            if (!ModelState.IsValid)
            {
                return View();

            }

            bool isExist = await _context.Tags.AnyAsync(t => t.Name == tag.Name);

            if (isExist)
            {
                ModelState.AddModelError("Name", "This Tag already existed!");
                return View();
            }

            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Tag tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id);
            if (tag is null) return NotFound();
            return View(tag);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, Tag newTag)
        {
           
            Tag oldTag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id);
            if (oldTag is null) return NotFound();
           
            if (!ModelState.IsValid)
            {
                return View(oldTag);
            }
            bool isExistName = await _context.Tags.AnyAsync(t => t.Name.Trim() == newTag.Name.Trim() && t.Id != id);
            if (isExistName)
            {
                ModelState.AddModelError("Name", "Tag with this name already exist!");
                return View();
            }
            

            if (oldTag.Name == newTag.Name.Trim()) return RedirectToAction(nameof(Index));

            oldTag.Name = newTag.Name;
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }

        
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            Tag tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id);

            if (tag is null) return NotFound();

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
