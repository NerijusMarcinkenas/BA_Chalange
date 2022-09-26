using BooksSpot.Core.Enums;
using BooksSpot.Core.Models;
using BooksSpot.Data.Interfaces;
using BooksSpot.Data.Repositories;
using BooksSpot.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace BooksSpot.Service.Services
{

    public class LendingService : ILendingService<Book, ApplicationUser>
    {
        private readonly IBooksRepository<Book> _booksRepository;
        private readonly IReaderRepository<ApplicationUser> _readerRepository;
        private readonly IConfiguration _configuration;

        public LendingService(IBooksRepository<Book> booksRepository,
                                IReaderRepository<ApplicationUser> readerRepository,                              
                                IConfiguration configuration)
        {
            _booksRepository = booksRepository;
            _readerRepository = readerRepository;
            _configuration = configuration;
            
        }

        public async Task<List<Book>> GetBooksByTakeCount(int takeCount = 25)
        {
            if (!_booksRepository.IsAnyBooks())
            {
                var memmoryData = new InMemmoryData(_booksRepository, _configuration);
                await memmoryData.ReadAllBooksFromJsonAndWriteToDb();
            }
            
            return await _booksRepository.GetSpecifiedCountBooksAsync(takeCount); 
        }

        public Task<Book?> GetBookByIdAsync(int id) => _booksRepository.GetBookByIdAsync(id);

        public Task<List<Book>> GetUserBooksAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return Task.FromResult(new List<Book>());
            }
            return _readerRepository.GetUserBooksAsync(userId);
        }

        public async Task<ResultExtension<Book>> LendBookAsync(string userId, Book bookToLend)
        {
            var resultExt = new ResultExtension<Book>();
            if (!BookIsAvailable(bookToLend))
            {
                return SetBookNotAvailableResult(bookToLend);
            }

            var existingUser = await _readerRepository.GetUserAsync(userId);
            if (existingUser != null)
            {
                bookToLend.Status = BookStatus.Borrowed;
                existingUser.UserBooks.Add(bookToLend);
                _booksRepository.Update(bookToLend);
                _booksRepository.CommitAsync().Wait();
                resultExt.Result = bookToLend;
                resultExt.Message = "Book borrowed successfully!";
                return resultExt;
            }

            resultExt.Result = null;
            resultExt.Error = "User not found";
            return resultExt;
        }

        public async Task<ResultExtension<Book>> MakeRezervationAsync(string userId, Book bookToRezerve)
        {
            if (!BookIsAvailable(bookToRezerve))
            {
                return SetBookNotAvailableResult(bookToRezerve);
            }

            var resultExt = new ResultExtension<Book>();
            var user = await _readerRepository.GetUserAsync(userId);
            if (user != null)
            {
                var info = new BookRezervation
                {
                    UserId = userId
                };

                bookToRezerve.BookRezervationInfo = info;
                bookToRezerve.Status = BookStatus.Reserved;

                user.UserBooks.Add(bookToRezerve);
                _booksRepository.Update(bookToRezerve);

                await _booksRepository.CommitAsync();
                resultExt.Result = bookToRezerve;
                resultExt.Message = $"Book is rezerved untill {info.RezervationExpirationDate:yyyy-MM-dd} succesffully";
                return resultExt;
            }

            resultExt.Result = null;
            resultExt.Error = "User not found";
            return resultExt;
        }

        public async Task<ResultExtension<Book>> ReturnBookAsync(string userId, Book bookToReturn)
        {
            var result = new ResultExtension<Book>();
            var existingUser = await _readerRepository.GetUserAsync(userId);
            if (existingUser == null)
            {
                result.Result = null;
                result.Message = "User not found";
                return result;
            }

            if (bookToReturn.BookRezervationInfo != null)
            {
                _readerRepository.CancelRezervation(bookToReturn.BookRezervationInfo);
                result.Message = "Book rezervation is canceled.";
            }
            else
            {
                result.Message = "Book is returned successfully.";
            }

            existingUser.UserBooks.Remove(bookToReturn);
            bookToReturn.Status = BookStatus.Available;
            _booksRepository.Update(bookToReturn);
            _booksRepository.CommitAsync().Wait();
            result.Result = bookToReturn;
            return result;
        }

        public async Task<ResultExtension<List<Book>>> GetBooksBySearchTypeAsync(string searchTerm, SearchType type = SearchType.All, int takeCount = 25)
        {
            var resultExt = new ResultExtension<List<Book>>();
            if (string.IsNullOrEmpty(searchTerm) || type.Equals(SearchType.All))
            {
                resultExt.Result = _booksRepository.GetSpecifiedCountBooksAsync(takeCount, searchTerm).Result.ToList();
                return resultExt;
            }

            if (type.Equals(SearchType.PublishedYear))
            {
               return await FormatDateAndReturnBooksIfSuccess(searchTerm);
            }           

            var searchMethods = GetSearchMethods();
            var method = searchMethods.SingleOrDefault(k => k.Key.Equals(type)).Value;
            resultExt.Result = await method.Invoke(searchTerm);
            return resultExt;
        }

        public async Task<List<Book>> GetBooksByFiltersAsync(BookStatus status, Genre genre, int takeCount = 25)
        {
            if (status.Equals(BookStatus.NotSet) && genre.Equals(Genre.NotSet))
            {
                return await _booksRepository.GetSpecifiedCountBooksAsync(takeCount);
            }
            if (status.Equals(BookStatus.NotSet))
            {
                return await _booksRepository.GetByGenreAsync(genre);
            }
            if (genre.Equals(Genre.NotSet))
            {
                return await _booksRepository.GetByStatusAsync(status);
            }                        
            return await _booksRepository.GetByGenreAndStatus(genre, status);
        }

        private Dictionary<SearchType, Func<string, Task<List<Book>>>> GetSearchMethods()
        {
            var dict = new Dictionary<SearchType, Func<string, Task<List<Book>>>>
            {
                { SearchType.Author, _booksRepository.GetByAuthorAsync },
                { SearchType.Title, _booksRepository.GetByTitleAsync },             
                { SearchType.Isbn, _booksRepository.GetByIsbnAsync },
                { SearchType.Publisher, _booksRepository.GetByPublisherAsync }
            };
            return dict;
        }

        private async Task<ResultExtension<List<Book>>> FormatDateAndReturnBooksIfSuccess(string searchTerm)
        {
            var resultExt = new ResultExtension<List<Book>>();

            var provider = CultureInfo.InvariantCulture;
            var format = "yyyy";
            DateTime date;
            try
            {
                date = DateTime.ParseExact(searchTerm, format, provider);
            }
            catch (FormatException e)
            {
                resultExt.Error = e.Message;
                resultExt.Message = $"Please enter year only";
                resultExt.Result = null;
                return resultExt;
            }
            resultExt.Result = await _booksRepository.GetByPublishedYearAsync(date);
            return resultExt;
        }

        private static ResultExtension<Book> SetBookNotAvailableResult(Book book)
        {
            var resultExt = new ResultExtension<Book>
            {
                Error = $"{book.Title} is not available",
                Result = null
            };
            return resultExt;
        }

        private static bool BookIsAvailable(Book book) => book.Status.Equals(BookStatus.Available);

    }
}
