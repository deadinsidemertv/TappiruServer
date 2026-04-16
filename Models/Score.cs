namespace TappiruServer.Models
{
    public class Score
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string MapHash { get; set; }
        public string MapName { get; set; }
        public int _Score { get; set; }
        public float Accuracy { get; set; }
        public int MaxCombo { get; set; }
        public int CompletedPhases { get; set; }
        public int FailedPhases { get; set; }
        public int CompletedChars { get; set; }
        public int FailedChars { get; set; }
        public DateTime PlayedAt { get; set; }

        public float TP {  get; set; }
    }
}
