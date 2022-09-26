

namespace BooksSpot.Service.Services
{
    public class ResultExtension<T> where T : class
    {
        public string Error { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public T? Result { get; set; } = null;
    }
}
