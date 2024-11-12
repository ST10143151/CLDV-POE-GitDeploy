using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ABCRetailers.Data;
using ABCRetailers.Models;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Threading.Tasks;

public class OrderController : Controller
{
    private readonly ApplicationDbContext _context;

    public OrderController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Order
    public async Task<IActionResult> Index(int userId)
    {
        var orders = await _context.Orders.Include(o => o.Items)
                                          .ThenInclude(i => i.Product)
                                          .Where(o => o.UserId == userId)
                                          .ToListAsync();
        return View(orders);
    }

    // POST: Order/Create
    [HttpPost]
    public async Task<IActionResult> Create(int userId)
    {
        var cart = await _context.Carts.Include(c => c.Items).ThenInclude(i => i.Product)
                                        .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null || !cart.Items.Any())
        {
            return RedirectToAction("Index", "Cart", new { userId });
        }

        var order = new Order
        {
            UserId = userId,
            OrderDate = DateTime.Now,
            Items = cart.Items.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                TotalPrice = i.Quantity * (decimal)i.Product.Price
            }).ToList(),
            TotalAmount = cart.Items.Sum(i => i.Quantity * (decimal)i.Product.Price)
        };


        _context.Orders.Add(order);

        // Clear the cart
        cart.Items.Clear();
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index), new { userId });
    }
}
