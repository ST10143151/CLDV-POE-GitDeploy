using Microsoft.AspNetCore.Mvc;
using ABCRetailers.Data;
using ABCRetailers.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

public class CartController : Controller
{
    private readonly ApplicationDbContext _context;

    public CartController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Cart
    public async Task<IActionResult> Index(int userId)
    {
        var cart = await _context.Carts
                                 .Include(c => c.Items)
                                 .ThenInclude(i => i.Product)
                                 .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            // Initialize a new empty cart if none exists
            cart = new Cart { UserId = userId, Items = new List<CartItem>() };
        }

        return View(cart);
    }

    // POST: Cart/AddToCart
    [HttpPost]
    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId, int userId, int quantity)
    {
        var cart = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            cart = new Cart { UserId = userId, Items = new List<CartItem>() };
            _context.Carts.Add(cart);
        }

        var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

        if (cartItem == null)
        {
            cartItem = new CartItem { ProductId = productId, Quantity = quantity, Cart = cart };
            cart.Items.Add(cartItem);
        }
        else
        {
            cartItem.Quantity += quantity;
        }

        await _context.SaveChangesAsync();

        // Redirect to the cart index to display the updated cart
        return RedirectToAction("Index", new { userId = userId });
    }


    // POST: Cart/RemoveFromCart
    [HttpPost]
    public async Task<IActionResult> RemoveFromCart(int productId, int userId)
    {
        var cart = await _context.Carts
                                 .Include(c => c.Items)
                                 .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null) return RedirectToAction(nameof(Index), new { userId });

        var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

        if (cartItem != null)
        {
            cart.Items.Remove(cartItem);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index), new { userId });
    }

    // POST: Cart/Checkout
    [HttpPost]
    public async Task<IActionResult> Checkout(int userId)
    {
        var cart = await _context.Carts.Include(c => c.Items).ThenInclude(i => i.Product)
                                        .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null || !cart.Items.Any())
        {
            return RedirectToAction(nameof(Index), new { userId });
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

        return RedirectToAction("Index", "Order", new { userId });
    }
}
