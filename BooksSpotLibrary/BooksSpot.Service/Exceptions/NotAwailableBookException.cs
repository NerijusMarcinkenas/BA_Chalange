using BooksSpot.Core.Enums;

namespace BooksSpot.Service.Exceptions
{
    public class NotAwailableBookException : Exception
    {
        public NotAwailableBookException(string bookName, BookStatus status)
            : base($"Book {bookName} is {nameof(status)}.")
        {
        }
    }
}
