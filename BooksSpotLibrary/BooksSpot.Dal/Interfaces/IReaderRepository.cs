using BooksSpot.Core.Models;

namespace BooksSpot.Data.Interfaces
{
    public interface IReaderRepository<TUser> where TUser : class
    {
        Task<TUser?> GetUserAsync(string id);

        Task<List<Book>> GetUserBooksAsync(string userId);
     
        BookRezervation CancelRezervation(BookRezervation bookRezervation);
    }
}
