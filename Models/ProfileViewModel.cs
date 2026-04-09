using System;
using System.Collections.Generic;

namespace TappiruServer.Models
{
    public class ProfileViewModel
    {
        // ===== ОСНОВНЫЕ ПОЛЯ (уже были) =====
        public string UserName { get; set; }
        public int Rating { get; set; }               // можно использовать как PP или как рейтинг
        public string? AvatarPath { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public int PlayCount { get; set; }
        public int AllTimeChar { get; set; }          // суммарное количество нажатий (хитов)
        public bool IsOwnProfile { get; set; }

        // ===== НОВЫЕ ПОЛЯ ДЛЯ OSU! СТИЛЯ =====

        // Обложка профиля
        public string? CoverUrl { get; set; }

        // Ранги
        public int GlobalRank { get; set; }            // мировой рейтинг (число)
        public int CountryRank { get; set; }           // рейтинг в стране
        public string CountryCode { get; set; }        // код страны (RU, US и т.д.)

        // Статистика
        public double PP { get; set; }                 // Performance Points
        public double Accuracy { get; set; }           // точность (доля, например 0.92841)
        public int Level { get; set; }                 // уровень игрока
        public double LevelProgress { get; set; }      // прогресс до следующего уровня (0–100)

        public int MaxCombo { get; set; }              // максимальное комбо
        public long TotalHits { get; set; }            // общее количество попаданий (300/100/50/miss)
        public long RankedScore { get; set; }          // рейтинговый счёт

        public int PlayTime { get; set; }              // время в игре (часы)

        // Личная информация
        public DateTime JoinDate { get; set; }          // дата регистрации (можно дублировать RegistrationDate)
        public string? Location { get; set; }           // местоположение (город/страна)
        public bool IsSupporter { get; set; }           // есть ли саппортер (донат)

        // Достижения (медали)
        public List<MedalDto> RecentAchievements { get; set; } = new();

        // История изменения ранга (последние 90 дней)
        public List<int> RankHistory { get; set; } = new();
    }

    // DTO для медалей/достижений
    public class MedalDto
    {
        public string Name { get; set; }
        public string IconUrl { get; set; }
        public string Description { get; set; }
    }
}