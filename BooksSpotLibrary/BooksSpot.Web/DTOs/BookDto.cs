using BooksSpot.Core.Enums;
using BooksSpot.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace BooksSpot.Web.DTOs
{
    public class BookDto : RezervationInfoDto
    {
        public int BookId { get; set; }
        public string Isbn { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public Genre Genre { get; set; }
        public DateTime DatePublished { get; set; }
        public BookStatus Status { get; set; }
        public BookDto()
        {
        }
        public BookDto(Book bookModel, BookRezervation? info) : base(info??new())
        {
            BookId = bookModel.Id;
            Isbn = bookModel.Isbn;
            Title = bookModel.Title;
            Author = bookModel.Author;
            Publisher = bookModel.Publisher;
            Genre = bookModel.Genre;
            DatePublished = bookModel.DatePublished;
            Status = bookModel.Status;
        }
        public BookDto(Book bookModel)
        {
            BookId = bookModel.Id;
            Isbn = bookModel.Isbn;
            Title = bookModel.Title;
            Author = bookModel.Author;
            Publisher = bookModel.Publisher;
            Genre = bookModel.Genre;
            DatePublished = bookModel.DatePublished;
            Status = bookModel.Status;
        }
    }
}
