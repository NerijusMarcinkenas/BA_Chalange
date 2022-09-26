using BooksSpot.Core.Enums;
using BooksSpot.Core.Models;
using BooksSpot.Dal.Data;
using BooksSpot.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BooksSpot.Data.Repositories
{
    public class BooksRepository : IBooksRepository<Book>
    {
        private readonly ApplicationDbContext _db;

        public BooksRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public Book AddToDb(Book bookToadd)
        {
            _db.Add(bookToadd);
            return bookToadd;
        }

        public Task<Book?> GetBookByIdAsync(int id) => _db.Books.Include(r => r.BookRezervationInfo).SingleOrDefaultAsync(i => i.Id == id);

        public Book Remove(Book objectToRemove)
        {
            _db.Remove(objectToRemove);
            return objectToRemove;
        }

        public Book Update(Book objectToUpdate)
        {
            var entity = _db.Attach(objectToUpdate);
            entity.State = EntityState.Modified;
            return objectToUpdate;
        }

        public Task<int> CommitAsync() => _db.SaveChangesAsync();

        public Task<List<Book>> GetAllAsync(string? searchTerm = null)
        {
            return _db.Books.Include(r => r.BookRezervationInfo).Where(x => string.IsNullOrWhiteSpace(searchTerm) ||
                                   x.Title.ToUpper().Contains(searchTerm.ToUpper()) ||
                                   x.Author.ToUpper().Contains(searchTerm.ToUpper()))
                                   .OrderBy(n => n.Title).ToListAsync();
        }

        public Task<List<Book>> GetByAuthorAsync(string author)
        {
            return _db.Books.Where(a => a.Author.ToUpper().Contains(author.ToUpper())).ToListAsync();
        }

        public Task<List<Book>> GetByGenreAsync(Genre genre)
        {
            return _db.Books.Where(g => g.Genre.Equals(genre)).ToListAsync();
        }

        public Task<List<Book>> GetByGenreAndStatus(Genre genre, BookStatus status)
        {
            return _db.Books.Where(g => g.Genre.Equals(genre) && g.Status.Equals(status)).ToListAsync();
        }

        public async Task<List<Book>> GetByIsbnAsync(string isbn)
        {
            var book = await _db.Books.SingleOrDefaultAsync(i => i.Isbn.Equals(isbn));
            if (book == null)
            {
                return new List<Book>();
            }
            return new List<Book>{ book };
        }

        public Task<List<Book>> GetByPublishedYearAsync(DateTime datePublished)
        {
            return  _db.Books.Where(d => d.DatePublished.Year.Equals(datePublished.Year)).ToListAsync();
        }

        public Task<List<Book>> GetByPublisherAsync(string publisher)
        {
            return _db.Books.Where(p => p.Publisher.ToUpper().Contains(publisher.ToUpper())).ToListAsync();
        }

        public Task<List<Book>> GetByStatusAsync(BookStatus status)
        {
            return _db.Books.Where(s => s.Status.Equals(status)).ToListAsync();
        }

        public Task<List<Book>> GetByTitleAsync(string title)
        {
            return _db.Books.Where(t => t.Title.ToUpper().Contains(title.ToUpper())).ToListAsync();
        }
        
        public Dictionary<SearchType, Func<string, Task<List<Book>>>> GetSearchMethods()
        {
            var dict = new Dictionary<SearchType, Func<string, Task<List<Book>>>>
            {
                { SearchType.Author, GetByAuthorAsync },               
                { SearchType.Title, GetByTitleAsync },
                { SearchType.All, GetAllAsync },
                { SearchType.Isbn, GetByIsbnAsync },
                { SearchType.Publisher, GetByPublisherAsync }
            };
            return dict;
        }
    }
}
