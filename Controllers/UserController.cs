using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TappiruServer.Models;
using TappiruServer.Models.DTO;           // ← поменяй, если у тебя DTOs

namespace TappiruServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "Неверный токен" });

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound(new { message = "Пользователь не найден" });

            var dto = new UserProfileDTO
            {
                UserName = user.UserName ?? "",
                Rating = user.Rating,
                AvatarPath = user.AvatarPath,
                GlobalRank = 0                     // Пока заглушка, позже сделаем реальный расчёт
            };

            return Ok(dto);
        }
    }
}