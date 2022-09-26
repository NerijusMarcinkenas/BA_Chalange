using BooksSpot.Core.Models;
using BooksSpot.Service.Interfaces;
using BooksSpot.Web.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BooksSpot.Web.Pages.Authorization
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILendingService<Book, ApplicationUser> lendingService;

        [BindProperty]
        public LoginDto LoginDto { get; set; } = new();

        public LoginModel(SignInManager<ApplicationUser> signInManager,
                            ILendingService<Book,ApplicationUser> lendingService)
        {
            _signInManager = signInManager;
            this.lendingService = lendingService;
        }

        public void OnGetAsync()
        {
            
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            var signInResult = await _signInManager.PasswordSignInAsync(LoginDto.Username, LoginDto.Password, LoginDto.RememberMe, false);
            
            if (signInResult.Succeeded)
            {
                if (String.IsNullOrEmpty(returnUrl) || returnUrl.Equals("/"))
                {
                    return Redirect("/Index");
                }
                return RedirectToPage(returnUrl);
            }

            ModelState.AddModelError("", "Uername or password incorect");
            return Page();
        }      
    }
}
