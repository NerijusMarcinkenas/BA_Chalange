using BooksSpot.Core.Enums;
using BooksSpot.Core.Models;
using BooksSpot.Service.Services;

namespace BooksSpot.Service.Interfaces
{
    public interface ILendingService<TBook, TUser>
                      where TBook : class
                      where TUser : class
    {
        Task<ResultExtension<TBook>> LendBookAsync(string userId, TBook bookToLend);

        Task<ResultExtension<TBook>> MakeRezervationAsync(string userId, TBook bookToRezerve);

        Task<ResultExtension<TBook>> ReturnBookAsync(string userId, TBook bookToReturn);

        Task<ResultExtension<List<TBook>>> GetBooksBySearchTypeAsync(string searchTerm, SearchType type = SearchType.All, int takeCount = 25);
        Task<List<TBook>> GetBooksByFiltersAsync(BookStatus status, Genre genre, int skipCount = 0);

        Task<TBook?> GetBookByIdAsync(int id);

        Task<List<TBook>> GetBooksByTakeCount(int takeCount = 25);

        Task<List<TBook>> GetUserBooksAsync(string userId);
    }
}
