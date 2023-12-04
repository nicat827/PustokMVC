using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PustokApp.DAL;
using PustokApp.Models;
using PustokApp.ViewModels;
using System.Net.Http.Headers;

namespace PustokApp.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _context;

        public BasketController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            string cooikes = Request.Cookies["basket"];
            List<BasketItemVM> basketItemVM = new List<BasketItemVM>();

            if (cooikes is not null)
            {
                List<CookieBasketItemVM> items = JsonConvert.DeserializeObject<List<CookieBasketItemVM>>(cooikes);

                foreach (var item in items)
                {
                    Book book = await _context.Books.Include(b => b.Images.Where(bi => bi.IsPrimary == true)).FirstOrDefaultAsync(b => b.Id == item.BookId);
                    if (book is not null)
                    {
                        basketItemVM.Add(new BasketItemVM
                        {
                            Id = book.Id,
                            Count = item.Count,
                            Name = book.Name,
                            Price = book.SalePrice - book.Discount,
                            Subtotal = item.Count * (book.SalePrice - book.Discount),
                            ImageUrl = book.Images[0].Image
                        });
                    }
                }

            }

            return View(basketItemVM);


        }


        public async Task<IActionResult> Add(int id)
        {
            if (id <= 0) return BadRequest();

            Book book = await _context.Books.Include(b => b.Images.Where(bi => bi.IsPrimary == true)).FirstOrDefaultAsync(b => b.Id == id);
            if (book is null) return NotFound();

            string cooikes = Request.Cookies["basket"];
            if (cooikes is null)
            {
                Response.Cookies.Append("basket", JsonConvert.SerializeObject(new List<CookieBasketItemVM> { new CookieBasketItemVM { BookId = id, Count = 1 } }));
                return RedirectToAction(nameof(Index), "Home");
            }

            List<CookieBasketItemVM> items = JsonConvert.DeserializeObject<List<CookieBasketItemVM>>(cooikes);
            CookieBasketItemVM existedItem = items.FirstOrDefault(i => i.BookId == id);
            if (existedItem is null)
            {
                items.Add(new CookieBasketItemVM { BookId = id, Count = 1 });
                Response.Cookies.Append("basket", JsonConvert.SerializeObject(items));
                return RedirectToAction(nameof(Index), "Home");


            }

            existedItem.Count++;
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(items));
            return RedirectToAction(nameof(Index), "Home");


          



        }


        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            Book book = await _context.Books.Include(b => b.Images.Where(bi => bi.IsPrimary == true)).FirstOrDefaultAsync(b => b.Id == id);
            if (book is null) return NotFound();
            string cooikes = Request.Cookies["basket"];
            if (cooikes is null) return NotFound();

            List<CookieBasketItemVM> items = JsonConvert.DeserializeObject<List<CookieBasketItemVM>>(cooikes);
            CookieBasketItemVM existedItem = items.FirstOrDefault(i => i.BookId == id);
            if (existedItem is null) return NotFound();

            items.Remove(existedItem);
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(items));
            return RedirectToAction(nameof(Index), "Home");



        }
    }
}
