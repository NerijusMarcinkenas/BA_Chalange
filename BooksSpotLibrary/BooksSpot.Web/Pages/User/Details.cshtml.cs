using BooksSpot.Core.Models;
using BooksSpot.Service.Interfaces;
using BooksSpot.Web.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BooksSpot.Web.Pages.User
{
    public class DetailsModel : PageModel
    {
        private readonly ILendingService<Book, ApplicationUser> _lendingService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public BookDto BookDto { get; set; } = new();
        public RezervationInfoDto RezervationInfoDto { get; set; } = new();
        public UserDto UserDto { get; set; } = new();
        public DetailsModel(ILendingService<Book, ApplicationUser> lendingService,
                            SignInManager<ApplicationUser> signInManager)
        {
            _lendingService = lendingService;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGetAsync(int bookId)
        {
            var existingBook = await _lendingService.GetBookByIdAsync(bookId);
              
            if (existingBook == null)
            {
                return Redirect("/Books/NotFound");

            }
            else if (existingBook.BookRezervationInfo != null)
            {
                RezervationInfoDto = new RezervationInfoDto(existingBook.BookRezervationInfo);                
            }

            var logedInUser = await _signInManager.UserManager.GetUserAsync(User);
            UserDto = new UserDto(logedInUser);
            BookDto = new BookDto(existingBook,existingBook.BookRezervationInfo);
            return Page();      
        }
    }
}
