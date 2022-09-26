using BooksSpot.Core.Enums;
using BooksSpot.Core.Models;
using BooksSpot.Service.Interfaces;
using BooksSpot.Web.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BooksSpot.Web.Pages.Books
{

    public class BooksListModel : PageModel
    {
        private readonly ILendingService<Book, ApplicationUser> _service;

        private readonly IHtmlHelper _htmlHelper;

        public string? Message = string.Empty;

        public string? Error = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public SearchType SearchType { get; set; }

        [BindProperty(SupportsGet = true)]
        public BookStatus BookStatus { get; set; }

        [BindProperty(SupportsGet = true)]
        public Genre Genre { get; set; }
       

        public IEnumerable<SelectListItem> SearchTypes { get; set; } = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> GenreFilter { get; set; } = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> BookStatusFilter { get; set; } = Enumerable.Empty<SelectListItem>();

        public List<BookDto> BooksDto { get; set; } = new();

        public BookDto BookDto { get; set; } = new();

        public BooksListModel(ILendingService<Book, ApplicationUser> service,
                             IHtmlHelper htmlHelper)
        {
            _service = service;
            _htmlHelper = htmlHelper;
        }

        public async Task<IActionResult> OnGet()
        {
            SetDropDownFields();

            if (TempData["Message"] != null)
            {
                Message = TempData["Message"]?.ToString();
            }

            if (TempData["Error"] != null)
            {
                Error = TempData["Error"]?.ToString();
            }
            var books = await _service.GetAllBooksAsync();
            BooksDto = books.Select(b => new BookDto(b)).ToList();
            return Page();
        }

        public async Task<IActionResult> OnGetSearchAsync()
        {
            SetDropDownFields();           
            var booksResult = await _service.GetBooksBySearchTypeAsync(SearchTerm, SearchType);

            if (booksResult.Result == null || !booksResult.Result.Any())
            {
                Error = booksResult.Error;
                Message = booksResult.Message;
                return Page();
            }
            BooksDto = booksResult.Result.Select(b => new BookDto(b)).ToList();
            return Page();
        }

        public async Task<IActionResult> OnGetFilterAsync()
        {
            SetDropDownFields();
          
            var booksFromDb = await _service.GetBooksByFiltersAsync(BookStatus, Genre);

            if (booksFromDb == null || !booksFromDb.Any())
            {
                TempData["Message"] = "Books not found";
                return Page();
            }
            BooksDto = booksFromDb.Select(b => new BookDto(b)).ToList();
            return Page();
        }

        private void SetDropDownFields()
        {
            SearchTypes = _htmlHelper.GetEnumSelectList<SearchType>();
            GenreFilter = _htmlHelper.GetEnumSelectList<Genre>();
            BookStatusFilter = _htmlHelper.GetEnumSelectList<BookStatus>();
        }
    }
}
