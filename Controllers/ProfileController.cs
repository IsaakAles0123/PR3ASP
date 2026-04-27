using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using PR1_ASP.Models;

namespace PR1_ASP.Controllers
{
    public class ProfileController : Controller
    {
        private const string ProfileCookieKey = "profile_input_json";

        [HttpGet]
        public IActionResult Index()
        {
            ViewData["Title"] = "Профиль";
            return View(new UserProfileInputViewModel());
        }

        [HttpPost]
        public IActionResult Index(UserProfileInputViewModel model)
        {
            ViewData["Title"] = "Профиль";

            if (!ModelState.IsValid)
                return View(model);

            
            var json = JsonSerializer.Serialize(model);
            var encoded = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(json));

            Response.Cookies.Append(ProfileCookieKey, encoded, new Microsoft.AspNetCore.Http.CookieOptions
            {
                HttpOnly = true,
                IsEssential = true,
                Expires = DateTimeOffset.UtcNow.AddDays(1)
            });

            return RedirectToAction(nameof(Result));
        }

        [HttpGet]
        public IActionResult Result()
        {
            ViewData["Title"] = "Введённая информация";

            var cookieValue = Request.Cookies[ProfileCookieKey];
            if (string.IsNullOrWhiteSpace(cookieValue))
                return View(new UserProfileDisplayViewModel { HasData = false });

            try
            {
                var jsonBytes = Convert.FromBase64String(cookieValue);
                var json = System.Text.Encoding.UTF8.GetString(jsonBytes);

                var data = JsonSerializer.Deserialize<UserProfileInputViewModel>(json);
                if (data is null)
                    return View(new UserProfileDisplayViewModel { HasData = false });

                
                if (string.IsNullOrWhiteSpace(data.FullName) ||
                    string.IsNullOrWhiteSpace(data.Email) ||
                    string.IsNullOrWhiteSpace(data.Bio))
                {
                    return View(new UserProfileDisplayViewModel { HasData = false });
                }

                return View(new UserProfileDisplayViewModel { HasData = true, Data = data });
            }
            catch
            {
                return View(new UserProfileDisplayViewModel { HasData = false });
            }
        }
    }
}

