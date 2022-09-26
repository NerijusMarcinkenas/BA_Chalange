using BooksSpot.Core.Models;
using BooksSpot.Service.Interfaces;
using BooksSpot.Web.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BooksSpot.Web.Pages.User
{
    public class MyBooksModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILendingService<Book, ApplicationUser> _lendingService;
        public List<BookDto> MyBooks { get; set; } = new();
        public MyBooksModel(SignInManager<ApplicationUser> signInManager,
                                ILendingService<Book, ApplicationUser> lendingService)
        {
            _signInManager = signInManager;
            _lendingService = lendingService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var userId = _signInManager.UserManager.GetUserId(HttpContext.User);
            var userBooks = await _lendingService.GetUserBooksAsync(userId);
           
            MyBooks = userBooks.Select(b => new BookDto(b,b.BookRezervationInfo)).ToList();            
            return Page();
        }
    }
}
