using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PustokApp.Models;

namespace PustokApp.DAL
{
    public class AppDbContext: IdentityDbContext<AppUser>
    {   
        public DbSet<Slide> Slides { get; set; }
        public DbSet<Feature> Features { get; set; }

        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookImage> BookImages { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<BookTag> BookTags { get; set; }



        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }
    }
}
