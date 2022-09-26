using System.ComponentModel.DataAnnotations;

namespace BooksSpot.Web.DTOs
{
    public class LoginDto
    {
        [Required]        
        public string Username { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
    }
}
