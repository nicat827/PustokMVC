using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PustokApp.DAL;
using PustokApp.Models;
using PustokApp.ViewModels;
using System.Security.Claims;

namespace PustokApp.Services
{
    public class LayoutService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _http;

        public LayoutService(AppDbContext context, IHttpContextAccessor http)
        {
            _context = context;
            _http = http;
        }

        public async Task<Dictionary<string, string>> GetSettingsAsync()
        {
            return await _context.Settings.ToDictionaryAsync(s => s.Key, s => s.Value);
        }

        public async Task<List<BasketItemVM>> GetBasketAsync()
        {
            List<BasketItemVM> basketItemVM = new List<BasketItemVM>();

            if (_http.HttpContext.User.Identity.IsAuthenticated)
            {
                List<BasketItem> userBasketItems = await _context.BasketItems
                    .Include(bi => bi.Book)
                    .ThenInclude(b => b.Images.Where(bi => bi.IsPrimary == true))
                    .Where(bi => bi.AppUserId == _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier))
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
                string cooikes = _http.HttpContext.Request.Cookies["basket"];
                if (!cooikes.IsNullOrEmpty())
                {
                    List<CookieBasketItemVM> items = JsonConvert.DeserializeObject<List<CookieBasketItemVM>>(cooikes);

                    foreach (var item in items)
                    {
                        Book book = await _context.Books.Where(b => b.IsDeleted == false).Include(b => b.Images.Where(bi => bi.IsPrimary == true)).FirstOrDefaultAsync(b => b.Id == item.BookId);
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
           

            return basketItemVM;
        }
    }
}
