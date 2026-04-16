namespace TappiruServer.Models.DTO
{
    public class SubmitScoreDTO
    {

        // Основная информация о карте
        public string MapName { get; set; } = string.Empty;     // Название песни/карты
        public string MapHash {  get; set; } = string.Empty;

        // Результаты прохождения
        public int Score { get; set; }                          // Набранные очки
        public float Accuracy { get; set; }                     // Точность от 0.0 до 1.0
        public int MaxCombo { get; set; }                       // Комбо в этой игре

        // Специфика твоей игры (печать)
        public int CompletedChars { get; set; }                 // Сколько символов правильно напечатал
        public int FailedChars { get; set; }                    // Сколько ошибок сделал

        // Если у тебя есть фазы в карте (например куплеты)
        public int CompletedPhases { get; set; } = 0;
        public int FailedPhases { get; set; } = 0;

        // Когда была сыграна карта
        public DateTime? PlayedAt { get; set; }
    }
}
