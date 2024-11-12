
namespace ABCRetailers.Models;
public class Order
{
    public int OrderId { get; set; }
    public int UserId { get; set; } // Or a User reference
    public DateTime OrderDate { get; set; }
    public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    public decimal TotalAmount { get; set; }
}
