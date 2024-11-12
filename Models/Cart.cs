
namespace ABCRetailers.Models;
public class Cart
{
    public int CartId { get; set; }
    public int UserId { get; set; } // Or a User reference if you have user authentication
    public List<CartItem> Items { get; set; } = new List<CartItem>();
}
