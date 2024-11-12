
using System.ComponentModel.DataAnnotations;

namespace ABCRetailers.Models
{
    public class Customer 
    {
        [Key]
        public int Customer_Id { get; set; }  // Ensure this property exists and is populated
        public string? Customer_Name { get; set; }  // Ensure this property exists and is populated
        public string? Email { get; set; }
        public string? Password { get; set; }

      

        public DateTimeOffset? Timestamp { get; set; }
    }
}
