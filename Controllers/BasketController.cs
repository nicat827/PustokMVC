using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PustokApp.DAL;
using PustokApp.Models;
using PustokApp.ViewModels;
using System.Net.Http.Headers;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace PustokApp.Controllers
{
    [AutoValidateAntiforgeryToken]

    public class BasketController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public BasketController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            List<BasketItemVM> basketItemVM = new List<BasketItemVM>();

            if (User.Identity.IsAuthenticated)
            {
                List<BasketItem> userBasketItems = await _context.BasketItems
                   .Include(bi => bi.Book)
                   .ThenInclude(b => b.Images.Where(bi => bi.IsPrimary == true))
                   .Where(bi => bi.AppUserId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                   .ToListAsync();
                foreach (BasketItem basketItemFromDb in userBasketItems)
                {
                    basketItemVM.Add(new BasketItemVM
                    {
                        Id = basketItemFromDb.Book.Id,
                        Count = basketItemFromDb.Count,
                        Name = basketItemFromDb.Book.Name,
                        ImageUrl = basketItemFromDb.Book.Images[0].Image,
                        Price = basketItemFromDb.Book.SalePrice - basketItemFromDb.Book.Discount,
                        Subtotal = (basketItemFromDb.Book.SalePrice - basketItemFromDb.Book.Discount) * basketItemFromDb.Count

                    });
                }
            }
            else
            {
                string cooikes = Request.Cookies["basket"];
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
            }
           

            return View(basketItemVM);


        }


        public async Task<IActionResult> Add(int id)
        {
            if (id <= 0) return BadRequest();

            Book book = await _context.Books.Include(b => b.Images.Where(bi => bi.IsPrimary == true)).FirstOrDefaultAsync(b => b.Id == id);
            if (book is null) return NotFound();
            List<BasketItemVM> basketItemVM = new List<BasketItemVM>();

            if (User.Identity.IsAuthenticated)
            {
                AppUser? user = await _userManager.Users
                    .Include(u => u.BasketItems)
                    .FirstOrDefaultAsync(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (user == null) return NotFound();
                BasketItem item = user.BasketItems.FirstOrDefault(bi => bi.BookId == id);
                if (item is null)
                {
                    user.BasketItems.Add(new BasketItem
                    {
                        Count = 1,
                        BookId = id

                    }) ;
                }
                else item.Count++;

                await _context.SaveChangesAsync();

                foreach (BasketItem basketItemFromDb in  user.BasketItems)
                {
                    Book existed = await _context.Books.Include(b => b.Images.Where(bi => bi.IsPrimary == true)).FirstOrDefaultAsync(b => b.Id == basketItemFromDb.BookId);
                    if (existed is not null)
                    {
                        basketItemVM.Add(new BasketItemVM
                        {
                            Id = existed.Id,
                            Count = basketItemFromDb.Count,
                            Name = existed.Name,
                            Price = existed.SalePrice - existed.Discount,
                            Subtotal = basketItemFromDb.Count * (existed.SalePrice - existed.Discount),
                            ImageUrl = existed.Images[0].Image
                        });
                    }
                }

            }
            else
            {
                string cooikes = Request.Cookies["basket"];
                List<CookieBasketItemVM> items = new List<CookieBasketItemVM>();
                if (cooikes is null)
                {
                    items.Add(new CookieBasketItemVM { BookId = id, Count = 1 });
                }

                else
                {
                    items = JsonConvert.DeserializeObject<List<CookieBasketItemVM>>(cooikes);
                    CookieBasketItemVM existedItem = items.FirstOrDefault(i => i.BookId == id);
                    if (existedItem is null)
                    {
                        items.Add(new CookieBasketItemVM { BookId = id, Count = 1 });

                    }
                    else
                    {
                        existedItem.Count++;
                    }


                }

                Response.Cookies.Append("basket", JsonConvert.SerializeObject(items));


                foreach (var item in items)
                {
                    Book existed = await _context.Books.Include(b => b.Images.Where(bi => bi.IsPrimary == true)).FirstOrDefaultAsync(b => b.Id == item.BookId);
                    if (existed is not null)
                    {
                        basketItemVM.Add(new BasketItemVM
                        {
                            Id = existed.Id,
                            Count = item.Count,
                            Name = existed.Name,
                            Price = existed.SalePrice - existed.Discount,
                            Subtotal = item.Count * (existed.SalePrice - existed.Discount),
                            ImageUrl = existed.Images[0].Image
                        });
                    }
                }

            }

            return PartialView("CartPartialView" ,basketItemVM);

		}


        public async Task<IActionResult> Delete(int id, string? returnUrl)
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
            if (returnUrl is not null)
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(Index), "Home");


        }


        public async Task<IActionResult> Decrease(int id)
        {
            if (id <= 0) return BadRequest();
            string cookies = Request.Cookies["basket"];
            if (cookies is null) return NotFound();
            List<CookieBasketItemVM> deserializedCookies = JsonConvert.DeserializeObject<List<CookieBasketItemVM>>(cookies);
            CookieBasketItemVM findedItem = deserializedCookies.FirstOrDefault(c => c.BookId == id);
            if (findedItem is null) return NotFound();
            if (findedItem.Count > 1)
            {
                findedItem.Count--;
                Response.Cookies.Append("basket", JsonConvert.SerializeObject(deserializedCookies));
                return RedirectToAction(nameof(Index));
            }
            deserializedCookies.Remove(findedItem);
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(deserializedCookies));
            return RedirectToAction(nameof(Index));


        }


        public async Task<IActionResult> Increase(int id)
        {
            if (id <= 0) return BadRequest();
			Book book = await _context.Books.Include(b => b.Images.Where(bi => bi.IsPrimary == true)).FirstOrDefaultAsync(b => b.Id == id);
			if (book is null) return NotFound();
			string cooikes = Request.Cookies["basket"];
			if (cooikes is null) return NotFound();
			List<CookieBasketItemVM> items = JsonConvert.DeserializeObject<List<CookieBasketItemVM>>(cooikes);
			CookieBasketItemVM existedItem = items.FirstOrDefault(i => i.BookId == id);
			if (existedItem is null)
			{
                return NotFound();

			}
			else
			{
				existedItem.Count++;
				Response.Cookies.Append("basket", JsonConvert.SerializeObject(items));
			}
            return RedirectToAction(nameof(Index));
		}
    }
}
