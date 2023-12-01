using Microsoft.EntityFrameworkCore;
using PustokApp.DAL;

namespace PustokApp.Services
{
    public class LayoutService
    {
        private readonly AppDbContext _context;

        public LayoutService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<string,string>> GetSettingsAsync()
        {
            return await _context.Settings.ToDictionaryAsync(s => s.Key, s => s.Value);
        }
    }
}
