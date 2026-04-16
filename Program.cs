using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TappiruServer.Data;
using TappiruServer.Models;

var builder = WebApplication.CreateBuilder(args);

// ====================== DbContext ======================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    string? connectionString;

    if (builder.Environment.IsProduction())
    {
        var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
        if (string.IsNullOrWhiteSpace(databaseUrl))
            throw new InvalidOperationException("DATABASE_URL is missing in Production!");

        connectionString = ConvertPostgresUrlToConnectionString(databaseUrl);
        options.UseNpgsql(connectionString, npgsql =>
        {
            npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "public");
        });
    }
    else
    {
        // Для разработки используем SQLite (как было раньше)
        connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("DefaultConnection string is missing in appsettings.json");

        options.UseSqlite(connectionString);
    }

    // Общие настройки
    options.ConfigureWarnings(warnings =>
    {
        warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning);
    });
    options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
});

static string ConvertPostgresUrlToConnectionString(string? url)
{
    if (string.IsNullOrWhiteSpace(url))
        throw new InvalidOperationException("DATABASE_URL environment variable is not set.");

    Console.WriteLine($"🔗 Raw DATABASE_URL: {url.Substring(0, Math.Min(80, url.Length))}...");

    var databaseUri = new Uri(url);

    var userInfo = databaseUri.UserInfo.Split(':', 2);
    if (userInfo.Length != 2)
        throw new InvalidOperationException("Invalid DATABASE_URL: cannot parse username/password.");

    var host = databaseUri.Host;
    var port = databaseUri.Port > 0 ? databaseUri.Port : 5432;   // важный фикс!
    var database = databaseUri.LocalPath.TrimStart('/');

    var connectionString =
        $"Host={host};" +
        $"Port={port};" +
        $"Database={database};" +
        $"Username={userInfo[0]};" +
        $"Password={userInfo[1]};" +
        "SSL Mode=Require;" +
        "Trust Server Certificate=true;";

    Console.WriteLine($"✅ Built connection string for host: {host}, port: {port}, db: {database}");

    return connectionString;
}


// ====================== Identity ======================
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// ====================== Cookie Authentication (для сайта) ======================
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Registration/Login";
    options.LogoutPath = "/Registration/Logout";
    options.Cookie.Name = "TappiruAuth";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(14);
    options.SlidingExpiration = true;
    options.Cookie.SameSite = SameSiteMode.Lax;        // важно для работы с играми/клиентами
});

// ====================== Authentication Schemes ======================
builder.Services.AddAuthentication(options =>
{
    // По умолчанию для MVC/сайта — используем Cookie от Identity
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
})
// JWT только для API (игра)
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"[JWT] Authentication Failed: {context.Exception.Message}");
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            Console.WriteLine($"[JWT] Challenge triggered: {context.Error}");
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    dbContext.Database.SetCommandTimeout(180);

    try
    {
        Console.WriteLine("🔄 Checking for pending migrations...");

        var pending = dbContext.Database.GetPendingMigrations().ToList();

        if (pending.Any())
        {
            Console.WriteLine($"📌 Applying {pending.Count} pending migration(s): {string.Join(", ", pending)}");
            dbContext.Database.Migrate();
            Console.WriteLine("✅ Migrations applied successfully!");
        }
        else
        {
            Console.WriteLine("✅ Database is up to date. No migrations to apply.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"⚠️ Migration error (ignored for startup): {ex.Message}");
        // Приложение продолжит работу даже при ошибке миграции
    }
}
// ====================== Middleware ======================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();

app.UseAuthentication();   // ← обязательно перед UseAuthorization
app.UseAuthorization();

app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();