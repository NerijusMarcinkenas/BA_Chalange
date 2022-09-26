using BooksSpot.Core.Models;
using BooksSpot.Service.Interfaces;
using BooksSpot.Web.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BooksSpot.Web.Pages.Books
{
    public class BorrowBookModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILendingService<Book,ApplicationUser> _lendingService;
        public BookDto Book { get; set; } = new();
        public BorrowBookModel(SignInManager<ApplicationUser> signInManager,
                                ILendingService<Book,ApplicationUser> lendingService)
        {
            _signInManager = signInManager;
            _lendingService = lendingService;
        }
        public async Task OnGetAsync(int bookId)
        {
            var existingBook = await _lendingService.GetBookByIdAsync(bookId);
            if (existingBook != null)
            {
                Book = new BookDto(existingBook);
            }             
        }
        
        public async Task<IActionResult> OnPostBorrow(int bookId)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return Redirect("/Authorization/Login");
            }
            
            var book = await _lendingService.GetBookByIdAsync(bookId);
            if (book == null)
            {
                return Redirect("/NotFound");
            }

            var userId = _signInManager.UserManager.GetUserId(HttpContext.User) ?? string.Empty;
            var result = await _lendingService.LendBookAsync(userId, book);
            if (result.Result == null)
            {
                TempData["Message"]= result.Message;
                TempData["Error"] = result.Error;
            }
            TempData["Message"] = result.Message;
            return Redirect("/Books/List");
        }

        public async Task<IActionResult> OnPostRezerve(int bookId)
        {
            var bookToRezerve = await _lendingService.GetBookByIdAsync(bookId);
            if (bookToRezerve == null)
            {
                return Redirect("/NotFound");
            }

            var userId = _signInManager.UserManager.GetUserId(HttpContext.User) ?? string.Empty;
            var result = await _lendingService.MakeRezervationAsync(userId, bookToRezerve);
            if (result.Result == null)
            {
                TempData["Message"] = result.Message;
                TempData["Error"] = result.Error;
            }
            TempData["Message"] = result.Message;
            return Redirect("/Books/List");
        }
    }
}
