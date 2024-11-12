using Microsoft.AspNetCore.Identity;

namespace ABCRetailers.Models
{
    public class User : IdentityUser
    {
        // Additional custom properties specific to your application
        public string? Name { get; set; }
        public string? Role { get; set; }
        // The role feature was kept for development purposes
        public string? Profile { get; set; }
        public string? Image { get; set; }
        
    }
}
