namespace BooksSpot.Service.Exceptions
{
    internal class BookNotFoundException : Exception
    {
        public BookNotFoundException(string searchParameter, string key) 
            : base($"Book not found searched by {searchParameter}, entered value: {key}")
        {          
        }
    }
}
