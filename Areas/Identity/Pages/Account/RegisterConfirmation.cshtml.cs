using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using ABCRetailers.Models; // Custom User model

namespace ABCRetailers.Areas.Identity.Pages.Account
{
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public RegisterConfirmationModel(UserManager<User> userManager)
        {
            _userManager = userManager;
            EmailConfirmationUrl = string.Empty; // Initialize to an empty string
        }
       // {
          //  _userManager = userManager;
       // }

        public bool DisplayConfirmAccountLink { get; set; }

        public string EmailConfirmationUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string email, string? returnUrl = null)
        {
            if (email == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Unable to load user with email '{email}'.");
            }

            DisplayConfirmAccountLink = false; // Set this based on your confirmation strategy
            if (!DisplayConfirmAccountLink)
            {
                EmailConfirmationUrl = Url?.Page("/Account/ConfirmEmail", pageHandler: null, values: new { area = "Identity", userId = user.Id, code = "sample-code" }, protocol: Request?.Scheme) ?? string.Empty;
            }

            return Page();
        }
    }
}
