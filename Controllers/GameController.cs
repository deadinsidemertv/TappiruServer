using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TappiruServer.Models;
using System.Threading.Tasks;

namespace TappiruServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Требует валидный JWT
    public class GameController : ControllerBase   // Лучше ControllerBase для API
    {
        private readonly UserManager<ApplicationUser> _userManager;

        // Конструктор — ASP.NET Core сам передаст UserManager
        public GameController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("submit-score")]
        public async Task<IActionResult> SubmitScore([FromBody] ScoreResult score)
        {
            // Получаем пользователя из JWT (который в заголовке Authorization)
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized(new { message = "Неверный токен или пользователь не найден" });

            // Обновляем статистику
            user.PlayCount++;
            user.AllTimeChar += score.EarnedPoints;
            // Можно также обновить рейтинг, например:
            // user.Rating = (user.Rating + score.EarnedPoints) / 2;

            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                message = "Результат сохранён",
                playCount = user.PlayCount,
                allTimeChar = user.AllTimeChar,
                rating = user.Rating
            });
        }
    }

    // Модель для данных, которые присылает игра
    public class ScoreResult
    {
        public int EarnedPoints { get; set; }
        public int Combo { get; set; }        // опционально
        public double Accuracy { get; set; }  // опционально
    }
}