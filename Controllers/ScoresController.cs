using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TappiruServer.Data;
using TappiruServer.Models;
using TappiruServer.Models.DTO;        // или .DTOs — как у тебя лежит

namespace TappiruServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ScoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ScoresController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("submit")]                    // ← явно указываем маршрут
        public async Task<IActionResult> SubmitScore([FromBody] SubmitScoreDTO dto)
        {
            if (dto == null)
                return BadRequest(new { message = "Данные не переданы" });

            // Получаем Id текущего пользователя из JWT
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "Неверный токен" });

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound(new { message = "Пользователь не найден" });

            // === Обновляем статистику пользователя ===
            user.PlayCount += 1;
            user.AllTimeChar += dto.CompletedChars;

            // Простая система повышения рейтинга (можно улучшить позже)
            int ratingGain = dto.Accuracy switch
            {
                > 0.95f => 25,
                > 0.90f => 18,
                > 0.80f => 12,
                > 0.70f => 7,
                _ => 3
            };

            user.Rating += ratingGain;

            if (dto.MaxCombo > user.MaxCombo)
                user.MaxCombo = dto.MaxCombo;

            // Сохраняем сам результат игры
            var score = new Score
            {
                UserId = userId,
                MapName = dto.MapName,
                MapHash = dto.MapHash,
                _Score = dto.Score,
                Accuracy = dto.Accuracy,
                MaxCombo = dto.MaxCombo,
                CompletedPhases = dto.CompletedPhases,
                FailedPhases = dto.FailedPhases,
                CompletedChars = dto.CompletedChars,
                FailedChars = dto.FailedChars,
                PlayedAt = dto.PlayedAt?.ToUniversalTime() ?? DateTime.UtcNow
            };

            _context.Scores.Add(score);

            await _context.SaveChangesAsync();
            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                message = "Результат успешно сохранён",
                rating = user.Rating,
                playCount = user.PlayCount,
                allTimeChar = user.AllTimeChar,
                ratingGain = ratingGain
            });
        }
    }
}