
using BooksSpot.Core.Models;
using BooksSpot.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BooksSpot.Data.Repositories
{
    public interface IMemmoryData
    {
        Task ReadAllBooksFromJsonAndWriteToDb(string? path = null);
    }

    public class InMemmoryData : IMemmoryData
    {
        private readonly IBooksRepository<Book> _booksRepository;
        private readonly string _path;

        public InMemmoryData(IBooksRepository<Book> booksRepository,
                               IConfiguration configuration)
        {
            _booksRepository = booksRepository;
            _path = configuration.GetSection("JsonBooksPath").Value;
        }

        public Task ReadAllBooksFromJsonAndWriteToDb(string? path = null)
        {
            string jsonBooks;

            if (path != null)
            {
               jsonBooks = File.ReadAllText(path);
            }
            else
            {
                jsonBooks = File.ReadAllText(_path);
            }
            
            var allBooks = JsonConvert.DeserializeObject<List<Book>>(jsonBooks) ?? new List<Book>();            
            foreach (var book in allBooks)
            {
                _booksRepository.AddToDb(book);
            }
            return _booksRepository.CommitAsync();
        }
    }
}
