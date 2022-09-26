using System.ComponentModel.DataAnnotations.Schema;

namespace BooksSpot.Core.Models
{
    public class BookRezervation
    {
        public int Id { get; set; }      
        public DateTime? RezerverdDate { get; set; } = null;        
        public DateTime? RezervationExpirationDate { get; set; } = null;        

        [ForeignKey("User")]
        public string? UserId { get; set; }

        [ForeignKey("Book")]
        public int? BookId { get; set; }

        public Book? Book { get; set; }
        public ApplicationUser? User { get; set; }

        public int ExpirationDays { get; private set; } = 14;
        public BookRezervation()
        {            
            RezervationExpirationDate = DateTime.UtcNow.AddDays(ExpirationDays);
            RezerverdDate = DateTime.UtcNow;
        }
    }
}
