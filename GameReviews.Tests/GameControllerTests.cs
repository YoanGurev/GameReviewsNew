using GameReviews.Controllers;
using GameReviews.Data;
using GameReviews.Models;
using GameReviews.Models.ViewModels;
using GameReviews.Services;
using GameReviews.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace GameReviews.Tests
{
    public class GamesControllerTests
    {
        private ApplicationDbContext GetInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        private Mock<UserManager<ApplicationUser>> MockUserManager()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            return new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
        }

        private IGameService GetGameService(ApplicationDbContext db)
        {
            
            var httpAccessor = new Mock<IHttpContextAccessor>();
            return new GameService(db, MockUserManager().Object, httpAccessor.Object);
        }

        private GamesController GetController(ApplicationDbContext db)
        {
            var userManager = MockUserManager();
            var service = GetGameService(db);
            return new GamesController(service, userManager.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewWithPaginatedList()
        {
            var db = GetInMemoryDb();
            var genre = new Genre { Id = 1, Name = "Action" };
            var platform = new Platform { Id = 1, Name = "PC" };
            db.Genres.Add(genre);
            db.Platforms.Add(platform);

            db.Games.AddRange(new List<Game>
            {
                new Game { Title = "Game A", Description = "Desc A", Genre = genre, Platform = platform },
                new Game { Title = "Game B", Description = "Desc B", Genre = genre, Platform = platform },
                new Game { Title = "Game C", Description = "Desc C", Genre = genre, Platform = platform }
            });
            db.SaveChanges();

            var controller = GetController(db);

            var result = await controller.Index(null, null, null, 1);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
        }

        [Fact]
        public async Task Details_ReturnsView_WhenGameExists()
        {
            var db = GetInMemoryDb();
            var genre = new Genre { Id = 1, Name = "Action" };
            var platform = new Platform { Id = 1, Name = "PC" };
            db.Genres.Add(genre);
            db.Platforms.Add(platform);

            db.Games.Add(new Game
            {
                Id = 1,
                Title = "Test Game",
                Description = "Description here",
                Genre = genre,
                Platform = platform
            });

            db.SaveChanges();

            var controller = GetController(db);

            var result = await controller.Details(1);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Game>(viewResult.Model);
            Assert.Equal("Test Game", model.Title);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenGameMissing()
        {
            var db = GetInMemoryDb();
            var controller = GetController(db);

            var result = await controller.Details(999);
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
