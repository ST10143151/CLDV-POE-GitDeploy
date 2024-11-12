using Microsoft.AspNetCore.Mvc;
using ABCRetailers.Data;
using ABCRetailers.Models;
using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;
using System.Linq;

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
        var cart = await _context.Carts.Include(c => c.Items).ThenInclude(i => i.Product)
                                        .FirstOrDefaultAsync(c => c.UserId == userId);
        return View(cart);
    }

    // POST: Cart/AddToCart
    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId, int userId, int quantity)
    {
        var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            cart = new Cart { UserId = userId };
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
        return RedirectToAction(nameof(Index), new { userId = userId });
    }
}
