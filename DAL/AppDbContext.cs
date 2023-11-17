﻿using Microsoft.EntityFrameworkCore;
using PustokApp.Models;

namespace PustokApp.DAL
{
    public class AppDbContext: DbContext
    {   
        public DbSet<Slide> Slides { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookImage> BookImages { get; set; }



        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }
    }
}
