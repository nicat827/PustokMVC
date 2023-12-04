using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokApp.DAL;
using PustokApp.Models;

namespace PustokApp.ViewComponents
{
    public class TagViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public TagViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            List<Book> relatedBooks = await _context.Books
                .Take(8)
                .Where(b => b.IsDeleted == false)
                .Include(b => b.BookTags.Where(x=>x.Tag.Name=="Long"))
                .ToListAsync();

            return View(relatedBooks);
        }
    }
}
