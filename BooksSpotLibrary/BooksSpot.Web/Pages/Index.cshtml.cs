using BooksSpot.Core.Models;
using BooksSpot.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BooksSpot.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILendingService<Book, ApplicationUser> _service;

        public IndexModel(ILogger<IndexModel> logger,
                            ILendingService<Book,ApplicationUser> service)
        {
            _logger = logger;
            _service = service;
        }

        public async Task OnGetAsync()
        {
            await _service.ReturnExpiredBooks();
        }
    }
}