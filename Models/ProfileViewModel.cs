using System;
using System.Collections.Generic;

namespace TappiruServer.Models
{
    public class ProfileViewModel
    {
        public string UserName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string? AvatarPath { get; set; }

        public int GlobalRank { get; set; } = 0;
        public int CountryRank { get; set; } = 0;
        public string CountryCode { get; set; } = "XX";

        public int PlayCount { get; set; }
        public long AllTimeChar { get; set; }
        public float Accuracy { get; set; }
        public int MaxCombo { get; set; }
        public int Level { get; set; } = 1;
        public float TotalPlayTime { get; set; }

        public DateTime? JoinDate { get; set; }

        public bool IsOwnProfile { get; set; }
        public bool IsSupporter { get; set; }
        public string? CoverUrl { get; set; }

        // Для графика (пока можно оставить пустым)
        public List<int> RankHistory { get; set; } = new();

        public List<TopScoreViewModel> TopScores { get; set; } = new List<TopScoreViewModel>();

        public class TopScoreViewModel
        {
            public string MapName { get; set; } = string.Empty;
            public string MapHash { get; set; } = string.Empty;
            public int Score { get; set; }
            public float Accuracy { get; set; }
            public int MaxCombo { get; set; }
            public float TP { get; set; }
            public DateTime PlayedAt { get; set; }
        }
    }
}