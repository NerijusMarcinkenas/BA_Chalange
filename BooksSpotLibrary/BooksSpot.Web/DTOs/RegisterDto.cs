using System.ComponentModel.DataAnnotations;

namespace BooksSpot.Web.DTOs
{
    public class UserRegisterDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]        
        public string Username { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords don't match")]
        public string ConfirmPassword { get; set; } = string.Empty;

    }
}
