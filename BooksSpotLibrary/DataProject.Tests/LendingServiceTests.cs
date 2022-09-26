using AutoFixture;
using AutoFixture.Xunit2;
using BooksSpot.Core.Enums;
using BooksSpot.Core.Models;
using BooksSpot.Data.Interfaces;
using BooksSpot.Data.Repositories;
using BooksSpot.Service.Interfaces;
using BooksSpot.Service.Services;
using Common;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Globalization;

namespace BooksSpotService.Tests
{
    public class LendingServiceTests
    {
        private readonly Mock<IBooksRepository<Book>> _booksRepositoryMock;
        private readonly Mock<IReaderRepository<ApplicationUser>> _readerRepositoryMock;
        private readonly Mock<IConfiguration> _configurationMock;    
        private readonly IFixture _fixture;
        private readonly ILendingService<Book, ApplicationUser> _sut;
        public LendingServiceTests()
        {
            _booksRepositoryMock = new Mock<IBooksRepository<Book>>() { CallBase = true };
            _readerRepositoryMock = new Mock<IReaderRepository<ApplicationUser>>();
            _configurationMock = new Mock<IConfiguration>();            
            _fixture = new Fixture();
            _sut = new LendingService(
                _booksRepositoryMock.Object,
                _readerRepositoryMock.Object,
                _configurationMock.Object);
            SetFixtureBehavior();
        }

        [Fact]
        public async Task GetBooksByTakeCount_ReturnsSpecifedCountOfBooks()
        {
            var booksMock = _fixture.Build<Book>().CreateMany(20).ToList();
            _booksRepositoryMock.Setup(i => i.IsAnyBooks()).Returns(true);
            _booksRepositoryMock.Setup(b => b.GetSpecifiedCountBooksAsync(int.MaxValue, null)).ReturnsAsync(booksMock);
            var resultBooks = await _sut.GetBooksByTakeCount(int.MaxValue);

            for (int i = 0; i < resultBooks.Count; i++)
            {
                Assert.Equal(booksMock[i].Id, resultBooks[i].Id);
            }
            _booksRepositoryMock.Verify(a => a.GetSpecifiedCountBooksAsync(int.MaxValue, null), Times.Once);
        }

        [Theory, Book]
        public async Task GetBookByIdAsync_ReturnsBookById(int id, Book bookMock)
        {          
            _booksRepositoryMock.Setup(g => g.GetBookByIdAsync(id)).ReturnsAsync(bookMock);
            var result = await _sut.GetBookByIdAsync(id);
            Assert.Equal(bookMock, result);
            _booksRepositoryMock.Verify(g => g.GetBookByIdAsync(id), Times.Once);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task GetUserBooksAsync_WhenUserIdIsNullOrEmpty_ReturnsEmptyList(string userId)
        {
            var result = await _sut.GetUserBooksAsync(userId);
            Assert.Empty(result);
            _readerRepositoryMock.Verify(g => g.GetUserBooksAsync(userId), Times.Never);
        }

        [Theory]
        [InlineAutoData]
        public async Task GetUserBooksAsync_WhenUserIdIsValid_ReturnsUserBooks(string userId)
        {
            var expected = _fixture.Build<Book>().CreateMany(10).ToList();
            _readerRepositoryMock.Setup(g => g.GetUserBooksAsync(userId)).ReturnsAsync(expected);
            var result = await _sut.GetUserBooksAsync(userId);

            for (int i = 0; i < result.Count; i++)
            {
                Assert.Equal(expected[i].Id, result[i].Id);
            }
            _readerRepositoryMock.Verify(g => g.GetUserBooksAsync(userId), Times.Once);
        }

        [Theory, Book]
        public async Task LendBookAsync_WhenBookIsNotAvailable_ReturnsResultNull(string userId, Book book)
        {
            book.Status = BookStatus.Borrowed;
            var result = await _sut.LendBookAsync(userId, book);

            Assert.Null(result.Result);
            Assert.Equal($"{book.Title} is not available", result.Error);
            _readerRepositoryMock.Verify(g => g.GetUserAsync(userId), Times.Never);
        }

        [Theory, Book]
        public async Task LendBookAsync_WhenUserIsNullAndBookIsAvailable_ReturnsResultNull(string userId, Book book)
        {
            ApplicationUser? userMock = null;
            _readerRepositoryMock.Setup(u => u.GetUserAsync(userId)).ReturnsAsync(userMock);
            userMock = _fixture.Build<ApplicationUser>().Create();

            var result = await _sut.LendBookAsync(userId, book);

            Assert.Null(result.Result);
            Assert.Equal("User not found", result.Error);
            _booksRepositoryMock.Verify(u => u.Update(book), Times.Never);
            _booksRepositoryMock.Verify(c => c.CommitAsync(), Times.Never);
        }

        [Theory, Book]
        public async Task LendBookAsync_WhenBookAndUserIsValid_ReturnsResultBookAndNoError(string userId, Book book)
        {
            var userMock = _fixture.Build<ApplicationUser>().Create();
            _readerRepositoryMock.Setup(u => u.GetUserAsync(userId)).ReturnsAsync(userMock);

            var result = await _sut.LendBookAsync(userId, book);

            Assert.NotNull(result.Result);
            Assert.Equal(book, result.Result);
            Assert.Equal("Book borrowed successfully!", result.Message);
            Assert.Empty(result.Error);
            _booksRepositoryMock.Verify(u => u.Update(book), Times.Once);
            _booksRepositoryMock.Verify(c => c.CommitAsync(), Times.Once);
        }

        [Theory, Book]
        public async Task MakeRezervationAsync_WhenBookAndUserIsValid_ReturnsResultBookAndNoError(string userId, Book book)
        {
            var userMock = _fixture.Build<ApplicationUser>().Create();
            _readerRepositoryMock.Setup(u => u.GetUserAsync(userId)).ReturnsAsync(userMock);

            var result = await _sut.MakeRezervationAsync(userId, book);

            Assert.NotNull(result.Result);
            Assert.Equal(book, result.Result);
            Assert.Empty(result.Error);
            _booksRepositoryMock.Verify(u => u.Update(book), Times.Once);
            _booksRepositoryMock.Verify(c => c.CommitAsync(), Times.Once);
        }

        [Theory, Book]
        public async Task ReturnBookAsync_WhenBookIsRezerved_ReturnsResultBookAndNoError(string userId, Book book)
        {
            book.Status = BookStatus.Reserved;

            var userMock = _fixture.Build<ApplicationUser>().Create();
            _readerRepositoryMock.Setup(u => u.GetUserAsync(userId)).ReturnsAsync(userMock);

            var result = await _sut.ReturnBookAsync(userId, book);

            Assert.NotNull(result.Result);
            Assert.Equal(book, result.Result);
            _booksRepositoryMock.Verify(u => u.Update(book), Times.Once);
            _booksRepositoryMock.Verify(c => c.CommitAsync(), Times.Once);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task GetBooksBySearchTypeAsync_WhenSearchTermIsNullOrEmpty_ReturnsAllBooksAsync(string searchTerm)
        {
            var booksMock = _fixture.CreateMany<Book>(10).ToList();
            _booksRepositoryMock.Setup(i => i.IsAnyBooks()).Returns(true);
            _booksRepositoryMock.Setup(a => a.GetSpecifiedCountBooksAsync(int.MaxValue, searchTerm)).ReturnsAsync(booksMock);

            var result = await _sut.GetBooksBySearchTypeAsync(searchTerm, default, int.MaxValue);

            Assert.NotNull(result.Result);
            var notNullResult = result.Result ?? new List<Book>();
            for (int i = 0; i < notNullResult.Count; i++)
            {
                Assert.Equal(booksMock[i].Id, notNullResult[i].Id);
            }
            _booksRepositoryMock.Verify(g => g.GetSpecifiedCountBooksAsync(int.MaxValue, searchTerm), Times.Once);
        }

        [Theory]
        [InlineData(SearchType.PublishedYear, "2022")]
        public async Task GetBooksBySearchTypeAsync_WhenSearchTypeDate_ReturnsBooksByPublishedDate(SearchType searchType, string searchTerm)
        {
            var date = DateTime.ParseExact(searchTerm, "yyyy", CultureInfo.InvariantCulture);
            var booksMock = _fixture.Build<Book>().With(d => d.DatePublished, date).CreateMany(1).ToList();
            _booksRepositoryMock.Setup(d => d.GetByPublishedYearAsync(date)).ReturnsAsync(booksMock);
            var year = date.Year.ToString();

            var result = await _sut.GetBooksBySearchTypeAsync(year, searchType);

            Assert.NotNull(result.Result);
            var notNullResult = result.Result ?? new List<Book>();
            for (int i = 0; i < notNullResult.Count; i++)
            {
                Assert.Equal(booksMock[i].Id, notNullResult[i].Id);
                Assert.Equal(booksMock[i].DatePublished, notNullResult[i].DatePublished);
            }
            _booksRepositoryMock.Verify(d => d.GetByPublishedYearAsync(date), Times.Once);
        }

        [Theory]
        [InlineData(BookStatus.NotSet, Genre.NotSet)]
        public async Task GetBooksByFilterAsync_WhenStatusAndGenreNotSet_ReturnsAllBooks(BookStatus status, Genre genre)
        {
            var booksMock = _fixture.CreateMany<Book>(10).ToList();            
            _booksRepositoryMock.Setup(a => a.GetSpecifiedCountBooksAsync(int.MaxValue, null)).ReturnsAsync(booksMock);

            var result = await _sut.GetBooksByFiltersAsync(status, genre, int.MaxValue);

            for (int i = 0; i < result.Count; i++)
            {
                Assert.Equal(booksMock[i].Id, result[i].Id);
            }
            _booksRepositoryMock.Verify(g => g.GetSpecifiedCountBooksAsync(int.MaxValue, null), Times.Once);
        }

        [Theory]
        [InlineData(BookStatus.NotSet, Genre.Classics)]
        public async Task GetBooksByFilterAsync_WhenOnlyStatusNotSet_ReturnsBooksByGenre(BookStatus status, Genre genre)
        {
            var booksMock = _fixture.CreateMany<Book>(10).ToList();
            _booksRepositoryMock.Setup(a => a.GetByGenreAsync(genre)).ReturnsAsync(booksMock);

            var result = await _sut.GetBooksByFiltersAsync(status, genre);

            for (int i = 0; i < result.Count; i++)
            {
                Assert.Equal(booksMock[i].Id, result[i].Id);
            }
            _booksRepositoryMock.Verify(g => g.GetByGenreAsync(genre), Times.Once);
        }

        [Theory]
        [InlineData(BookStatus.Available, Genre.NotSet)]
        public async Task GetBooksByFilterAsync_WhenOnlyGenreNotSet_ReturnsBooksByStatus(BookStatus status, Genre genre)
        {
            var booksMock = _fixture.CreateMany<Book>(10).ToList();
            _booksRepositoryMock.Setup(a => a.GetByStatusAsync(status)).ReturnsAsync(booksMock);

            var result = await _sut.GetBooksByFiltersAsync(status, genre);

            for (int i = 0; i < result.Count; i++)
            {
                Assert.Equal(booksMock[i].Id, result[i].Id);
            }
            _booksRepositoryMock.Verify(g => g.GetByStatusAsync(status), Times.Once);
        }

        [Theory]
        [InlineData(BookStatus.Available, Genre.Classics)]
        public async Task GetBooksByFilterAsync_WhenStatusAndGenreSet_ReturnsBooksByBothValues(BookStatus status, Genre genre)
        {
            var booksMock = _fixture.CreateMany<Book>(10).ToList();
            _booksRepositoryMock.Setup(a => a.GetByGenreAndStatus(genre, status)).ReturnsAsync(booksMock);

            var result = await _sut.GetBooksByFiltersAsync(status, genre);

            for (int i = 0; i < result.Count; i++)
            {
                Assert.Equal(booksMock[i].Id, result[i].Id);
            }
            _booksRepositoryMock.Verify(g => g.GetByGenreAndStatus(genre, status), Times.Once);
        }


        private void SetFixtureBehavior()
        {
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }
    }
}