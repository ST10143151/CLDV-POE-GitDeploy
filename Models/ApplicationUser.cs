using Microsoft.AspNetCore.Identity;

namespace ABCRetailers_Latest.Models
{
    

    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}

