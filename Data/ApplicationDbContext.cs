using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TappiruServer.Models;

namespace TappiruServer.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Score> Scores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);     // ← Это обязательно!

            // Дополнительная настройка для PostgreSQL (чтобы bool были настоящими boolean)
            modelBuilder.UseIdentityColumns();      // для автоинкремента

            // Если хочешь явно указать типы колонок (на всякий случай)
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.EmailConfirmed).HasColumnType("boolean");
                entity.Property(e => e.PhoneNumberConfirmed).HasColumnType("boolean");
                entity.Property(e => e.TwoFactorEnabled).HasColumnType("boolean");
                entity.Property(e => e.LockoutEnabled).HasColumnType("boolean");
            });

            // Конфигурация для Score (если нужно)
            modelBuilder.Entity<Score>(entity =>
            {
                entity.Property(s => s.PlayedAt).HasColumnType("timestamp with time zone");
            });
        }
    }
}