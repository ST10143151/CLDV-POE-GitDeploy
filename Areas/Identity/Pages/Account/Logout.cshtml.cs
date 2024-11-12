// Code Attribution:

//Allen, D. and Foster, A., 2022. Pro ASP.NET Core Identity: Under the Hood of Authentication and Authorization 
//for ASP.NET Core Applications. New York: Apress.
//Esposito, D., 2021. Modern Web Development with ASP.NET Core 5: An end-to-end guide to becoming a professional 
//full-stack web developer. Birmingham: Packt Publishing.
//Johnson, M., 2019. Improving efficiency through digital claims management. International Journal of Systems and 
//Applications, 7(4), pp.102-115.
//Microsoft, 2023. ASP.NET Core MVC Overview. Microsoft Docs. Available at: 
//https://learn.microsoft.com/en-us/aspnet/core/mvc/overview?view=aspnetcore-8.0 [Accessed 18 October 2024].
//Troelsen, A. and Japikse, P., 2021. Pro C# 9 with .NET 5: Foundational Principles and Practices in Programming.
//Full code attribution in Readme.md

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ABCRetailers_Latest.Models;  // Use your custom User model

namespace ABCRetailers_Latest.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LogoutModel(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Index");
        }
    }
}
