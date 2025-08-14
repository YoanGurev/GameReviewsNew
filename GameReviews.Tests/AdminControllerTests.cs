using Xunit;
using Moq;
using GameReviews.Areas.Admin.Controllers;
using GameReviews.Data;
using GameReviews.Models;
using GameReviews.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GameReviews.Tests
{
    public class AdminGamesControllerTests
    {
        private ApplicationDbContext GetInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(System.Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        private async Task SeedGenresAndPlatforms(ApplicationDbContext db)
        {
            db.Genres.AddRange(new List<Genre>
            {
                new Genre { Id = 1, Name = "Action" },
                new Genre { Id = 2, Name = "RPG" }
            });

            db.Platforms.AddRange(new List<Platform>
            {
                new Platform { Id = 1, Name = "PC" },
                new Platform { Id = 2, Name = "Xbox" }
            });

            await db.SaveChangesAsync();
        }

        private AdminGamesController GetController(ApplicationDbContext db)
        {
            var service = new AdminGameService(db);
            return new AdminGamesController(service);
        }

        [Fact]
        public async Task Index_ReturnsViewWithGames()
        {
            var db = GetInMemoryDb();
            await SeedGenresAndPlatforms(db);

            db.Games.Add(new Game { Id = 1, Title = "Game A", GenreId = 1, PlatformId = 1, Description = "Test A" });
            db.Games.Add(new Game { Id = 2, Title = "Game B", GenreId = 2, PlatformId = 2, Description = "Test B" });
            await db.SaveChangesAsync();

            var controller = GetController(db);

            var result = await controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Game>>(viewResult.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async Task Create_Get_ReturnsViewWithDropdowns()
        {
            var db = GetInMemoryDb();
            await SeedGenresAndPlatforms(db);

            var controller = GetController(db);
            var result = await controller.Create();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.ViewData["Genres"]);
            Assert.NotNull(viewResult.ViewData["Platforms"]);
        }

        [Fact]
        public async Task Create_Post_AddsGameAndRedirects()
        {
            var db = GetInMemoryDb();
            await SeedGenresAndPlatforms(db);
            var controller = GetController(db);

            var newGame = new Game
            {
                Title = "New Game",
                GenreId = 1,
                PlatformId = 2,
                Description = "New Game Description"
            };

            var result = await controller.Create(newGame);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(AdminGamesController.Index), redirectResult.ActionName);

            var gameInDb = await db.Games.FirstOrDefaultAsync(g => g.Title == "New Game");
            Assert.NotNull(gameInDb);
        }

        [Fact]
        public async Task Edit_Get_ReturnsViewWithGameAndDropdowns()
        {
            var db = GetInMemoryDb();
            await SeedGenresAndPlatforms(db);

            var game = new Game { Id = 1, Title = "Edit Game", GenreId = 1, PlatformId = 1, Description = "Desc" };
            db.Games.Add(game);
            await db.SaveChangesAsync();

            var controller = GetController(db);
            var result = await controller.Edit(game.Id);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Game>(viewResult.Model);
            Assert.Equal(game.Title, model.Title);

            Assert.NotNull(viewResult.ViewData["Genres"]);
            Assert.NotNull(viewResult.ViewData["Platforms"]);
        }

        [Fact]
        public async Task Edit_Post_UpdatesGameAndRedirects()
        {
            var db = GetInMemoryDb();
            await SeedGenresAndPlatforms(db);

            var game = new Game { Id = 1, Title = "Old Title", GenreId = 1, PlatformId = 1, Description = "Old Desc" };
            db.Games.Add(game);
            await db.SaveChangesAsync();
            db.Entry(game).State = EntityState.Detached;

            var controller = GetController(db);

            var updatedGame = new Game
            {
                Id = 1,
                Title = "Updated Title",
                GenreId = 2,
                PlatformId = 2,
                Description = "Updated Desc"
            };

            var result = await controller.Edit(game.Id, updatedGame);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(AdminGamesController.Index), redirectResult.ActionName);

            var gameInDb = await db.Games.FindAsync(game.Id);
            Assert.Equal("Updated Title", gameInDb.Title);
            Assert.Equal(2, gameInDb.GenreId);
            Assert.Equal(2, gameInDb.PlatformId);
        }

        [Fact]
        public async Task DeleteConfirmed_RemovesGameAndRedirects()
        {
            var db = GetInMemoryDb();
            await SeedGenresAndPlatforms(db);

            var game = new Game { Id = 1, Title = "To Delete", GenreId = 1, PlatformId = 1, Description = "To delete" };
            db.Games.Add(game);
            await db.SaveChangesAsync();

            var controller = GetController(db);
            var result = await controller.DeleteConfirmed(game.Id);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(AdminGamesController.Index), redirectResult.ActionName);

            var gameInDb = await db.Games.FindAsync(game.Id);
            Assert.Null(gameInDb);
        }
    }
}

