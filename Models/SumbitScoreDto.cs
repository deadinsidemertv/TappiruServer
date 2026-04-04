namespace TappiruServer.Models
{
    public class SumbitScoreDto
    {

        public float Accuracy { get; set; }
        public int MaxCombo { get; set; }
        public int Score { get; set; }          
        public int CompletedPhases { get; set; }
        public int FailedPhases { get; set; }
        public int CompletedChars { get; set; }
        public int FailedChars { get; set; }
        public DateTime PlayedAt { get; set; }
        public string MapName { get; set; }
    }
}
