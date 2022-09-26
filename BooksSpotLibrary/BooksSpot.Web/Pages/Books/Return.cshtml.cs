using BooksSpot.Core.Models;
using BooksSpot.Service.Interfaces;
using BooksSpot.Web.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BooksSpot.Web.Pages.Books
{
    public class ReturnModel : PageModel
    {
        private readonly ILendingService<Book, ApplicationUser> _lendingService;
        private readonly UserManager<ApplicationUser> _userManager;


        public BookDto BookToReturnDto { get; set; } = new();
        public string Handler = string.Empty;
        public ReturnModel(ILendingService<Book, ApplicationUser> lendingService,
                            UserManager<ApplicationUser> userManager)
        {
            _lendingService = lendingService;
            _userManager = userManager;
        }
        public async Task<IActionResult> OnGet(int bookId, string handler)
        {
            Handler = handler;
            var existingBook = await _lendingService.GetBookByIdAsync(bookId);            
            if (existingBook == null)
            {
                return RedirectToPage("/Books/NotFound");
            }
            BookToReturnDto = new BookDto(existingBook);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int bookId)
        {            
            var bookToReturn = await _lendingService.GetBookByIdAsync(bookId);
            if (bookToReturn == null)
            {
                return Redirect("/Books/NotFound");
            }

            var userId = _userManager.GetUserId(HttpContext.User);
            var result = await _lendingService.ReturnBookAsync(userId, bookToReturn);
            if (result.Result == null)
            {
                TempData["Error"] = result.Error;
            }
            TempData["Message"] = result.Message;            
            return Redirect("/User/MyBooks");
        }
    }
}
