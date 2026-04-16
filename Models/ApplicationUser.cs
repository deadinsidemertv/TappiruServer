using Microsoft.AspNetCore.Identity;

namespace TappiruServer.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int Rating { get; set; } = 0;
        public string? AvatarPath { get; set; }

        public DateTime? RegistrationDate { get; set; } = DateTime.UtcNow;

        public int PlayCount { get; set; } = 0;        // Сколько раз играл
        public long AllTimeChar { get; set; } = 0;     // Всего напечатанных символов за всё время
        public int MaxCombo { get; set; } = 0;         // Самое большое комбо за всю историю
        public float Accuracy { get; set; } = 0f;      // Общая точность 
        public float TotalPlayTime { get; set; } = 0f; // Время в игре

    }
}
