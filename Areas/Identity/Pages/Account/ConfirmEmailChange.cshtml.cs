using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace ABCRetailers.Areas.Identity.Pages.Account
{
    public class ConfirmEmailChangeModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public ConfirmEmailChangeModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public string? StatusMessage { get; set; }  // Nullable

        public async Task<IActionResult> OnGetAsync(string userId, string email, string code)
        {
            if (userId == null || code == null || email == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            var result = await _userManager.ChangeEmailAsync(user, email, code);
            if (!result.Succeeded)
            {
                StatusMessage = "Error changing email.";
                return Page();
            }

            await _userManager.SetUserNameAsync(user, email);
            StatusMessage = "Thank you for confirming your email change.";
            return Page();
        }
    }
}
