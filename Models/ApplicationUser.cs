using Microsoft.AspNetCore.Identity;

namespace TappiruServer.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int Rating { get; set; } = 0;
        public string? AvatarPath { get; set; }

        public DateTime? RegistrationDate { get; set; }

        public int PlayCount { get; set; } = 0;

        public int AllTimeChar { get; set; } = 0;

    }
}
