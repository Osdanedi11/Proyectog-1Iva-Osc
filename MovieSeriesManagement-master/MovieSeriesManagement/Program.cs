using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MovieSeriesManagement.Data;
using MovieSeriesManagement.Data.Repositories;
using MovieSeriesManagement.Models.Entities;
using MovieSeriesManagement.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
  options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Modifica la configuración de Identity para ser más permisiva con los requisitos
builder.Services.AddDefaultIdentity<ApplicationUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;

    // Configuración de contraseña menos estricta para desarrollo
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;

    // Configuración de usuario
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

// Register repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IContentRepository, ContentRepository>();
builder.Services.AddScoped<IViewingHistoryRepository, ViewingHistoryRepository>();
builder.Services.AddScoped<IRecommendationRepository, RecommendationRepository>();

// Register services
builder.Services.AddScoped<IContentService, ContentService>();
builder.Services.AddScoped<IRecommendationService, RecommendationService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IViewingHistoryService, ViewingHistoryService>();

// Agregar el servicio de email
builder.Services.AddTransient<IEmailSender, EmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
  name: "default",
  pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    await SeedData(serviceProvider);
}

// Asegurarse de que existan las carpetas necesarias
var webRootPath = app.Environment.WebRootPath;
var profileImagesPath = Path.Combine(webRootPath, "images", "profiles");
var contentImagesPath = Path.Combine(webRootPath, "images", "content");

if (!Directory.Exists(profileImagesPath))
{
    Directory.CreateDirectory(profileImagesPath);
}

if (!Directory.Exists(contentImagesPath))
{
    Directory.CreateDirectory(contentImagesPath);
}

app.Run();

// Seed data method
async Task SeedData(IServiceProvider serviceProvider)
{
    var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    // Ensure database is created
    context.Database.EnsureCreated();

    // Seed roles
    var roles = new[] { "Admin", "User" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Seed admin user
    string adminEmail = "admin@movieflix.com";
    string adminPassword = "Admin123!";

    // Check if admin exists
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        // Create admin user
        var admin = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
            FirstName = "Admin",
            LastName = "System",
            ProfilePictureUrl = "/images/profiles/default-profile.jpg"
        };

        var result = await userManager.CreateAsync(admin, adminPassword);

        if (result.Succeeded)
        {
            // Assign admin role
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }

    // Seed content if empty
    if (!context.Contents.Any())
    {
        await SeedContentData(context);
    }
}

// Seed content data
async Task SeedContentData(ApplicationDbContext context)
{
    // Movie genres
    var movieGenres = new[]
    {
        "Acción", "Aventura", "Comedia", "Drama", "Ciencia Ficción",
        "Terror", "Romance", "Animación", "Thriller", "Documental"
    };

    // Series genres
    var seriesGenres = new[]
    {
        "Drama", "Comedia", "Ciencia Ficción", "Fantasía", "Acción",
        "Crimen", "Misterio", "Documental", "Animación", "Aventura"
    };

    // Platforms
    var platforms = new[]
    {
        "Netflix", "Amazon Prime", "Disney+", "HBO Max", "Apple TV+",
        "Hulu", "Paramount+", "Peacock", "Crunchyroll", "YouTube Premium"
    };

    // Default image URL for all content
    string defaultImageUrl = "https://media.istockphoto.com/id/1147544807/vector/thumbnail-image-vector-graphic.jpg?s=612x612&w=0&k=20&c=rnCKVbdxqkjlcs3xH87-9gocETqpspHFXu5dIGB4wuM=";

    // Random for generating data
    var random = new Random();

    // Generate 50 movies
    var movies = new List<Content>();
    for (int i = 1; i <= 50; i++)
    {
        var releaseYear = random.Next(1980, 2024);
        var genre = movieGenres[random.Next(movieGenres.Length)];
        var platform = platforms[random.Next(platforms.Length)];

        movies.Add(new Content
        {
            Title = $"Película {i}: El Desafío",
            Genre = genre,
            Platform = platform,
            Type = ContentType.Movie,
            Description = $"Una emocionante película de {genre.ToLower()} que te mantendrá al borde de tu asiento. Estrenada en {releaseYear} y disponible en {platform}.",
            ReleaseDate = new DateTime(releaseYear, random.Next(1, 13), random.Next(1, 29)),
            ImageUrl = defaultImageUrl
        });
    }

    // Generate 50 series
    var series = new List<Content>();
    for (int i = 1; i <= 50; i++)
    {
        var startYear = random.Next(2000, 2023);
        var genre = seriesGenres[random.Next(seriesGenres.Length)];
        var platform = platforms[random.Next(platforms.Length)];
        var seasons = random.Next(1, 10);

        series.Add(new Content
        {
            Title = $"Serie {i}: Nuevos Horizontes",
            Genre = genre,
            Platform = platform,
            Type = ContentType.Series,
            Description = $"Una fascinante serie de {genre.ToLower()} con {seasons} temporadas. Comenzó en {startYear} y está disponible en {platform}.",
            ReleaseDate = new DateTime(startYear, random.Next(1, 13), random.Next(1, 29)),
            ImageUrl = defaultImageUrl
        });
    }

    // Add all content to database
    await context.Contents.AddRangeAsync(movies);
    await context.Contents.AddRangeAsync(series);
    await context.SaveChangesAsync();
}

