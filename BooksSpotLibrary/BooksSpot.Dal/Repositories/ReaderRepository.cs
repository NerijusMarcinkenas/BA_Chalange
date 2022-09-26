using BooksSpot.Core.Models;
using BooksSpot.Dal.Data;
using BooksSpot.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BooksSpot.Data.Repositories
{
    public class ReaderRepository : IReaderRepository<ApplicationUser>
    {
        private readonly ApplicationDbContext _db;

        public ReaderRepository(ApplicationDbContext db)
        {
            _db = db;
        }
              

        public Task<ApplicationUser?> GetUserAsync(string id) =>
             _db.Users.Include(b => b.UserBooks).SingleOrDefaultAsync(i => i.Id == id);
               
        public Task<List<Book>> GetUserBooksAsync(string userId)
        {
            return _db.Books.Include(b => b.BookRezervationInfo).Where(u => u.UserId == userId).ToListAsync();
        }

        public BookRezervation CancelRezervation(BookRezervation bookRezervation)
        {
            var entity = _db.Remove(bookRezervation);
            entity.State = EntityState.Deleted;
            return bookRezervation;
        }
    }
}
