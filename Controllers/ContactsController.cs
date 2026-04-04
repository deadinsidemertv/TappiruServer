using Microsoft.AspNetCore.Mvc;
using TappiruServer.Models;

namespace TappiruServer.Controllers
{
    public class ContactsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Check(Feedback feedback)
        {
            if (ModelState.IsValid)
                return Redirect("/");

            return View("Index");
        }
    }
}
