using AutoFixture.Kernel;
using BooksSpot.Core.Enums;
using BooksSpot.Core.Models;

namespace Common
{
    public class BookSpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is Type type && type == typeof(Book))
            {
                return new Book
                {
                    Id = 1,
                    Status = BookStatus.Available,
                    Title = "TestTitle",
                    Author = "TestAuthor",
                    Genre = Genre.Classics,
                    DatePublished = DateTime.Now,
                    Isbn = "TestIsbn",
                    UserId = "TestId",
                    Publisher = "TestBublisher",
                    BookRezervationInfo = new BookRezervation(),                    
                    User = new ApplicationUser(),                    
                };
            }
            return new NoSpecimen();
        }       
    }
}
