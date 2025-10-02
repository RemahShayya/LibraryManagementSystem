using LibraryManagementSystem.Data.Entities;
using LibraryManagementSystem.Data.Entities.ImageEntities;
using LibraryManagementSystem.Data.Identity;
using LibraryManagementSystem.Data.UserRoleData;
using LibraryManagmentSystem.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace LibraryManagmentSystem.Data
{
    public class LibraryContext : IdentityDbContext<User, Role, string>
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RoleConfiguration());

            modelBuilder.Entity<BookRentals>()
                .HasOne(br => br.Book)
                .WithMany(b => b.BookRentals)
                .HasForeignKey(br => br.BookId);

            modelBuilder.Entity<BookRentals>()
                .HasOne(br => br.Customer)
                .WithMany(u => u.BookRentals)
                .HasForeignKey(br => br.CustomerId);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithOne(a => a.Book)
                .HasForeignKey<Author>(a => a.BookId);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Publisher)
                .WithMany(p => p.Books)
                .HasForeignKey(b => b.PublisherId);

            modelBuilder.Entity<BookCategory>()
                .HasKey(bc => new { bc.BookId, bc.CategoryId });

            modelBuilder.Entity<BookCategory>()
                .HasOne(bc => bc.Book)
                .WithMany(b => b.BookCategories)
                .HasForeignKey(bc => bc.BookId);

            modelBuilder.Entity<BookCategory>()
                .HasOne(bc => bc.Category)
                .WithMany(c => c.BookCategories)
                .HasForeignKey(bc => bc.CategoryId);

            modelBuilder.Entity<Author>()
                .HasOne(a => a.AuthorImage)
                .WithOne(ai => ai.Author)
                .HasForeignKey<AuthorImage>(ai => ai.AuthorId);

            modelBuilder.Entity<Book>()
             .HasMany(b => b.BookImages)
             .WithOne(i => i.Book)
             .HasForeignKey(i => i.BookId)
             .OnDelete(DeleteBehavior.Cascade);
        }


        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }
        public DbSet<AuthorImage> AuthorImages { get; set; }
        public DbSet<BookImages> BookImages { get; set; }
        public DbSet<BookRentals> BookRentals { get; set; }
        public DbSet<ReturnedRental> ReturnedRentals { get; set; }
    }
}