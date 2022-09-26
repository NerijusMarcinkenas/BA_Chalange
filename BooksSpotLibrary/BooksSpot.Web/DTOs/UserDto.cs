using BooksSpot.Core.Models;

namespace BooksSpot.Web.DTOs
{
    public class UserDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserDto()
        {
        }
        public UserDto(ApplicationUser user)
        {
            Name = user.Name;
            Email = user.Email;
        }
    }
}
