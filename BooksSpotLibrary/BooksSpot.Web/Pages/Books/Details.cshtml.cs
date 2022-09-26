using BooksSpot.Core.Models;
using BooksSpot.Service.Interfaces;
using BooksSpot.Web.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BooksSpot.Web.Pages.Books
{
    public class DetailsModel : PageModel
    {
        private readonly ILendingService<Book, ApplicationUser> _lendingService;

        public BookDto BookDto { get; set; } = new();
        public DetailsModel(ILendingService<Book, ApplicationUser> lendingService)
        {
            _lendingService = lendingService;           
        }

        public async Task<IActionResult> OnGetAsync(int bookId)
        {            
            var existingBook = await _lendingService.GetBookByIdAsync(bookId);
            if (existingBook == null)
            {
                return Redirect("/NotFound");
            }
            BookDto = new BookDto(existingBook);
            return Page();
        }
    }
}
