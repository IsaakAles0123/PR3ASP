using Microsoft.AspNetCore.Mvc;
using PR1_ASP.Models;
using System.Diagnostics;

namespace PR1_ASP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
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