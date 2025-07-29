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
            if (!context.Genres.Any())
            {
                context.Genres.AddRange(
                    new Genre { Name = "Action" },
                    new Genre { Name = "Adventure" },
                    new Genre { Name = "RPG" },
                    new Genre { Name = "Shooter" },
                    new Genre { Name = "Strategy" }
                );
            }

            // Seed Platforms
            if (!context.Platforms.Any())
            {
                context.Platforms.AddRange(
                    new Platform { Name = "PC" },
                    new Platform { Name = "PlayStation" },
                    new Platform { Name = "Xbox" },
                    new Platform { Name = "Nintendo Switch" },
                    new Platform { Name = "Mobile" }
                );
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

