using BooksSpot.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BooksSpot.Core.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(13)]
        public string Isbn { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public Genre Genre { get; set; }

        [DataType(DataType.Date)]
        public DateTime DatePublished { get; set; }
        public BookStatus Status { get; set; }

        [ForeignKey("User")]
        public string? UserId { get; set; } = null;

        public BookRezervation? BookRezervationInfo { get; set; } = null;
        public ApplicationUser? User { get; set; } = null;

        public Book()
        {
        }

        public Book(string isbn, 
                    string title, 
                    string author, 
                    string publisher,
                    Genre genre, 
                    DateTime datePublished, 
                    BookStatus bookStatus)
        {
            Isbn = isbn;
            Title = title;
            Author = author;
            Publisher = publisher;
            Genre = genre;
            DatePublished = datePublished;
            Status = bookStatus;
        }
    }
}
