namespace TappiruServer.Models.DTO
{
    public class UserProfileDTO
    {
        public string UserName { get; set; } = string.Empty;
        public int Rating { get; set; } = 0;
        public string? AvatarPath { get; set; }
        public int GlobalRank { get; set; } = 0;

    }
}
