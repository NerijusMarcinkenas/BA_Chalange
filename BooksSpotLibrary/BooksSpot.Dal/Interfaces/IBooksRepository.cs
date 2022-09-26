using BooksSpot.Core.Enums;
using BooksSpot.Core.Models;

namespace BooksSpot.Data.Interfaces
{
    public interface IBooksRepository<TBook> where TBook : class
    {
        TBook AddToDb(TBook objectToAdd);

        TBook Remove(TBook objectToRemove);

        TBook Update(TBook objectToUpdate);

        Task<TBook?> GetBookByIdAsync(int id);

        Task<List<TBook>> GetSpecifiedCountBooksAsync(int takeCount = 25, string? searchTerm = null);

        Task<List<TBook>> GetByTitleAsync(string title);

        Task<List<TBook>> GetByAuthorAsync(string author);

        Task<List<TBook>> GetByPublisherAsync(string publisher);

        Task<List<TBook>> GetByPublishedYearAsync(DateTime publishingDate);

        Task<List<TBook>> GetByGenreAsync(Genre genre);

        Task<List<TBook>> GetByIsbnAsync(string isbn);

        Task<List<TBook>> GetByStatusAsync(BookStatus status);

        Task<List<Book>> GetByGenreAndStatus(Genre genre, BookStatus status);

        Task<int> CommitAsync();

        int GetBooksCount();
        bool IsAnyBooks();
    }
}
