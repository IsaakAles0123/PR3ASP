using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PR1_ASP.Models;
using System.Diagnostics;

namespace PR1_ASP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SneakerShopContext _db;

        public HomeController(ILogger<HomeController> logger, SneakerShopContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var featured = await _db.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .OrderBy(p => p.ProductName)
                .Take(3)
                .ToListAsync(cancellationToken);

            return View(new HomeIndexViewModel
            {
                FeaturedProducts = featured.Select(p => p.ToCatalogViewModel()).ToList()
            });
        }

        public IActionResult Privacy()
        {
            // Старый маршрут из шаблона MVC. Каталог вынесен в CatalogController.
            return RedirectToAction(nameof(CatalogController.Index), "Catalog");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}