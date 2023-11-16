using Microsoft.EntityFrameworkCore;
using PustokApp.Models;

namespace PustokApp.DAL
{
    public class AppDbContext: DbContext
    {   
        public DbSet<Slide> Slides { get; set; }
        public DbSet<Feature> Features { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }
    }
}
