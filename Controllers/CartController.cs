using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PR1_ASP.Models;

namespace PR1_ASP.Controllers;

public class CartController : Controller
{
    private const string CartIdCookieKey = "cart_id";

    private readonly SneakerShopContext _db;

    public CartController(SneakerShopContext db)
    {
        _db = db;
    }

    private bool TryGetCartIdFromCookie(out int cartId)
    {
        cartId = 0;
        return Request.Cookies.TryGetValue(CartIdCookieKey, out var raw) && int.TryParse(raw, out cartId);
    }

    private int GetOrCreateCartId()
    {
        if (TryGetCartIdFromCookie(out var existing))
            return existing;

        var cart = new Cart();
        _db.Carts.Add(cart);
        _db.SaveChanges();

        Response.Cookies.Append(CartIdCookieKey, cart.CartId.ToString(), new CookieOptions
        {
            HttpOnly = true,
            IsEssential = true,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        });

        return cart.CartId;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Корзина";

        if (!TryGetCartIdFromCookie(out var cartId))
            return View(new CartPageViewModel());

        var items = await _db.CartItems
            .Include(ci => ci.Product)
                .ThenInclude(p => p!.Category)
            .Where(ci => ci.CartId == cartId)
            .OrderBy(ci => ci.CartItemId)
            .ToListAsync();

        var vm = new CartPageViewModel();
        foreach (var ci in items)
        {
            if (ci.Product is null)
                continue;

            var categoryName = ci.Product.Category?.CategoryName ?? "-";
            vm.Lines.Add(new CartLineViewModel
            {
                CartItemId = ci.CartItemId,
                ProductId = ci.Product.ProductId,
                ProductName = ci.Product.ProductName,
                CategoryName = categoryName,
                Description = ProductDescriptionText.Short(ci.Product.ProductName, categoryName, ci.Product.Stock),
                Price = ci.Product.Price,
                Quantity = ci.Quantity,
                MaxStock = ci.Product.Stock
            });
        }

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Add(int productId, int quantity = 1)
    {
        if (quantity < 1)
            quantity = 1;

        var product = await _db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.ProductId == productId);
        if (product is null)
            return NotFound();

        var cartId = GetOrCreateCartId();

        var line = await _db.CartItems.FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
        var desired = line is null ? quantity : line.Quantity + quantity;
        var clamped = Math.Clamp(desired, 1, Math.Max(1, product.Stock));

        if (line is null)
        {
            _db.CartItems.Add(new CartItem
            {
                CartId = cartId,
                ProductId = productId,
                Quantity = clamped
            });
        }
        else
        {
            line.Quantity = clamped;
        }

        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Update(int cartItemId, int quantity)
    {
        if (!TryGetCartIdFromCookie(out var cartId))
            return RedirectToAction(nameof(Index));

        if (quantity < 1)
            quantity = 1;

        var line = await _db.CartItems
            .Include(ci => ci.Product)
            .FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId && ci.CartId == cartId);

        if (line?.Product is null)
            return NotFound();

        line.Quantity = Math.Clamp(quantity, 1, Math.Max(1, line.Product.Stock));
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Remove(int cartItemId)
    {
        if (!TryGetCartIdFromCookie(out var cartId))
            return RedirectToAction(nameof(Index));

        var line = await _db.CartItems.FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId && ci.CartId == cartId);
        if (line is null)
            return NotFound();

        _db.CartItems.Remove(line);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
