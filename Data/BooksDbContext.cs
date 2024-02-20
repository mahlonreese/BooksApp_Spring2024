using BooksApp_Spring2024.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BooksApp_Spring2024.Data
{
    //allows us to talk to the database
    public class BooksDbContext : IdentityDbContext<IdentityUser> //DbContext is a child class
    {
        public BooksDbContext(DbContextOptions<BooksDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; } //adds a table to the database

        public DbSet<Book> Books { get; set; } //adds the Books table to the database

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        //allows us to put in seed data    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Category>().HasData(

                new Category { CategoryID = 1, Name = "Science Fiction", Description = "This is the description for the Science Fiction Category" },

                new Category { CategoryID = 2, Name = "Technology", Description = "This is the description for the Technology Category" },

                new Category { CategoryID = 3, Name = "History", Description = "This is the description for the History Category" }

                );


            modelBuilder.Entity<Book>().HasData(

                new Book { BookId = 1, BookTitle = "Great Expectations", Author = "Charles Dickens", Description = "13t Century Novel about educating an orphan named pip", Price = 19.99m, ISBN = "IUTH829JD", CategoryID = 3, ImgUrl = ""},

                new Book { BookId = 2, BookTitle = "The Cat in the Hat", Author = "Dr. Seuss", Description = "Childrens Novel about a talking cat", Price = 11.99m, ISBN = "IUTH913KNB", CategoryID = 1, ImgUrl = "" },

                new Book { BookId = 3, BookTitle = "Brown Bear, Brown Bear, What do you see", Author = "Bill Martin", Description = "A childrens book about colors", Price = 5.99m, ISBN = "IUTH848DIW", CategoryID = 2, ImgUrl = "" }



                );

        }

    }
}
