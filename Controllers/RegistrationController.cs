using Microsoft.AspNetCore.Mvc;
using TappiruServer.Models;

namespace TappiruServer.Controllers
{
    public class RegistrationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Здесь будет сохранение в базу данных (позже)
                // Пока просто выведем сообщение
                TempData["Success"] = "Регистрация успешна! Теперь вы можете войти.";
                return RedirectToAction("Login");
            }
            return View(model);
        }

        public IActionResult Login()
        {
            return Content("Страница входа будет позже");
        }
    }
}
