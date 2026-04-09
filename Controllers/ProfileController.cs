using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TappiruServer.Data;
using TappiruServer.Models;

namespace TappiruServer.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        public ProfileController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<IActionResult> Profile(string userName)
        {
            // Если userName не передан – показываем профиль текущего пользователя
            var currentUser = await _userManager.GetUserAsync(User);
            ApplicationUser targetUser;

            if (string.IsNullOrEmpty(userName))
            {
                targetUser = currentUser;
            }
            else
            {
                targetUser = await _userManager.FindByNameAsync(userName);
                if (targetUser == null) return NotFound();
            }

            var model = new ProfileViewModel
            {
                UserName = targetUser.UserName,
                AvatarPath = targetUser.AvatarPath,   // предполагаем, что у ApplicationUser есть поле AvatarUrl
                Rating = targetUser.Rating,         // или вычислите из рейтинговой системы
                IsOwnProfile = (targetUser.Id == currentUser.Id)
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadAvatar(IFormFile avatarFile)
        {
            // Проверка, авторизован ли пользователь
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Challenge();

            // Валидация файла
            if (avatarFile == null || avatarFile.Length == 0)
            {
                TempData["Error"] = "Пожалуйста, выберите файл.";
                return RedirectToAction("Profile");
            }

            // Разрешённые расширения
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(avatarFile.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
            {
                TempData["Error"] = "Разрешены только изображения (jpg, png, gif).";
                return RedirectToAction("Profile");
            }

            // Ограничение размера (например, 5 МБ)
            if (avatarFile.Length > 5 * 1024 * 1024)
            {
                TempData["Error"] = "Размер файла не должен превышать 5 МБ.";
                return RedirectToAction("Profile");
            }

            // Папка для аватаров: wwwroot/avatars
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "avatars");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // Генерируем уникальное имя файла, чтобы избежать конфликтов
            var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Сохраняем файл
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await avatarFile.CopyToAsync(stream);
            }

            // Если у пользователя уже был аватар — удаляем старый файл (чтобы не засорять диск)
            if (!string.IsNullOrEmpty(user.AvatarPath))
            {
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.AvatarPath.TrimStart('/'));
                if (System.IO.File.Exists(oldFilePath))
                    System.IO.File.Delete(oldFilePath);
            }

            // Обновляем путь в базе данных (храним относительный путь)
            user.AvatarPath = $"/avatars/{uniqueFileName}";
            await _userManager.UpdateAsync(user);

            TempData["Success"] = "Аватар успешно обновлён!";
            return RedirectToAction("Profile");
        }
    }
}
