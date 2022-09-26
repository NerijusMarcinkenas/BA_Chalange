using BooksSpot.Core.Enums;
using Microsoft.AspNetCore.Identity;


namespace BooksSpot.Core.Models
{
    public class ApplicationUser : IdentityUser
    {      
        public string Name { get; set; } = string.Empty;
        public List<Book> UserBooks { get; set; } = new(); 
        
    }
}
