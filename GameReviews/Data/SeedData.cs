using GameReviews.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GameReviews.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Apply pending migrations
            await context.Database.MigrateAsync();

            // Seed Genres
            var allGenres = new[] {
                    "Action", "Adventure", "Role-Playing (RPG)", "Simulation", "Strategy",
                    "Shooter", "First Person Shooter (FPS)", "Puzzle", "Horror", "Platformer",
                    "Fighting", "Sports", "Racing", "MMO", "Sandbox", "Survival", "Stealth",
                    "Visual Novel", "Rhythm", "Card Game", "Trivia"
            };

            foreach (var genreName in allGenres)
            {
                if (!context.Genres.Any(g => g.Name == genreName))
                {
                    context.Genres.Add(new Genre { Name = genreName });
                }
            }


            // Seed Platforms
            var allPlatforms = new[] {
                   "PC", "PlayStation 5", "PlayStation 4", "Xbox 360", "Xbox Series X/S",
                   "Xbox One", "Nintendo Switch", "Nintendo NES", "GameCube", "Nintendo 64",
                   "Wii", "Game Boy", "Game Boy Advance", "Nintendo DS", "Nintendo 3DS",
                   "Mobile (iOS/Android)", "Mac", "Linux", "Browser", "Steam Deck",
                   "VR (Oculus)", "VR (HTC Vive)", "VR (Valve Index)", "VR (Meta Quest)"
            };

            foreach (var platformName in allPlatforms)
            {
                if (!context.Platforms.Any(p => p.Name == platformName))
                {
                    context.Platforms.Add(new Platform { Name = platformName });
                }
            }


            await context.SaveChangesAsync();

            // Seed Roles
            var roles = new[] { "Administrator", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Seed Admin User
            string adminEmail = "admin@gamereviews.com";
            string adminPassword = "Admin123!";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Administrator");
                }
            }
        }
    }
}

