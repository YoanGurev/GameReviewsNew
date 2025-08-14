using GameReviews.Controllers;
using GameReviews.Data;
using GameReviews.Models;
using GameReviews.Services;
using GameReviews.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace GameReviews.Tests
{
    public class FavoritesControllerTests
    {
        private ApplicationDbContext GetInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(System.Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        private Mock<UserManager<ApplicationUser>> MockUserManager()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            return new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
        }

        private IFavoritesService GetFavoritesService(ApplicationDbContext db)
        {
            // Use your actual FavoritesService
            return new FavoritesService(db);
        }

        private FavoritesController GetController(ApplicationDbContext db, Mock<UserManager<ApplicationUser>> userManager)
        {
            var service = GetFavoritesService(db);
            return new FavoritesController(service, userManager.Object);
        }

        [Fact]
        public async Task Toggle_AddsFavorite_WhenNotExists()
        {
            var db = GetInMemoryDb();
            var user = new ApplicationUser { Id = "user1" };
            var game = new Game
            {
                Id = 1,
                Title = "Test Game",
                Description = "Test Description",
                Genre = new Genre { Id = 1, Name = "Genre1" },
                Platform = new Platform { Id = 1, Name = "Platform1" }
            };
            db.Genres.Add(game.Genre);
            db.Platforms.Add(game.Platform);
            db.Games.Add(game);
            await db.SaveChangesAsync();

            var userManager = MockUserManager();
            userManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            var controller = GetController(db, userManager);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, user.Id) }))
                }
            };

            var result = await controller.Toggle(game.Id);

            Assert.Single(db.Favorites);
            var favorite = db.Favorites.First();
            Assert.Equal(game.Id, favorite.GameId);
            Assert.Equal(user.Id, favorite.UserId);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Details", redirect.ActionName);
            Assert.Equal(game.Id, redirect.RouteValues["id"]);
        }

        [Fact]
        public async Task Toggle_RemovesFavorite_WhenExists()
        {
            var db = GetInMemoryDb();
            var user = new ApplicationUser { Id = "user1" };
            var game = new Game
            {
                Id = 1,
                Title = "Test Game",
                Description = "Test Description",
                Genre = new Genre { Id = 1, Name = "Genre1" },
                Platform = new Platform { Id = 1, Name = "Platform1" }
            };
            db.Genres.Add(game.Genre);
            db.Platforms.Add(game.Platform);
            db.Games.Add(game);
            db.Favorites.Add(new Favorite { GameId = game.Id, UserId = user.Id });
            await db.SaveChangesAsync();

            var userManager = MockUserManager();
            userManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            var controller = GetController(db, userManager);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, user.Id) }))
                }
            };

            var result = await controller.Toggle(game.Id);

            Assert.Empty(db.Favorites);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Details", redirect.ActionName);
            Assert.Equal(game.Id, redirect.RouteValues["id"]);
        }
    }
}
