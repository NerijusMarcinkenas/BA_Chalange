using BooksSpot.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BooksSpot.Dal.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {     
        public DbSet<Book> Books => Set<Book>();

        public DbSet<BookRezervation> BookRezervations => Set<BookRezervation>();

        public new DbSet<ApplicationUser> Users => Set<ApplicationUser>();

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityUser>().ToTable("Users", "dbo");
        }
    }
}