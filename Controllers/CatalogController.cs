using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PR1_ASP.Models;

namespace PR1_ASP.Controllers;

public class CatalogController : Controller
{
    private readonly SneakerShopContext _db;

    public CatalogController(SneakerShopContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Каталог";

        var items = await _db.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .OrderBy(p => p.ProductName)
            .ToListAsync();

        return View(items.Select(p => p.ToCatalogViewModel()).ToList());
    }
}
