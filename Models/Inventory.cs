using ABCRetailers_Latest.Models;

namespace ABCRetailers_Latest.Models
{
    public class Inventory
    {
        public int InventoryId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public Product Product { get; set; }
    }
}


