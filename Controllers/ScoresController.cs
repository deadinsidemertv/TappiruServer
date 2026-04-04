using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TappiruServer.Data;
using TappiruServer.Models;

namespace TappiruServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // только авторизованные пользователи
    public class ScoresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ScoresController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpPost]
        public async Task<IActionResult> SubmitScore([FromBody] SumbitScoreDto dto)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return Unauthorized();

            var score = new Score
            {
                UserId = userId,
                MapName = dto.MapName,
                _Score = dto.Score,
                Accuracy = dto.Accuracy,
                MaxCombo = dto.MaxCombo,
                CompletedPhases = dto.CompletedPhases,
                FailedPhases = dto.FailedPhases,
                CompletedChars = dto.CompletedChars,
                FailedChars = dto.FailedChars,
                PlayedAt = dto.PlayedAt
            };

            _context.Scores.Add(score);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Результат сохранён" });
        }
    

    }
}
