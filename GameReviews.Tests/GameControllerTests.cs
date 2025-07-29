using Xunit;
using Moq;
using GameReviews.Controllers;
using GameReviews.Data;
using GameReviews.Models;
using GameReviews.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System;

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

            var userManager = MockUserManager();
            var controller = new GamesController(db, userManager.Object);

            var result = await controller.Index(null, null, null, 1);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
        }

        [Fact]
        public async Task Index_FiltersByGenrePlatformAndSearch()
        {
            var db = GetInMemoryDb();

            var genre1 = new Genre { Id = 1, Name = "Action" };
            var genre2 = new Genre { Id = 2, Name = "RPG" };
            var platform1 = new Platform { Id = 1, Name = "PC" };
            var platform2 = new Platform { Id = 2, Name = "Xbox" };

            db.Genres.AddRange(genre1, genre2);
            db.Platforms.AddRange(platform1, platform2);

            db.Games.AddRange(
                new Game { Id = 1, Title = "Action Game", Description = "Good", Genre = genre1, Platform = platform1 },
                new Game { Id = 2, Title = "RPG Game", Description = "Great", Genre = genre2, Platform = platform2 },
                new Game { Id = 3, Title = "Mixed Game", Description = "Okay", Genre = genre1, Platform = platform2 }
            );

            await db.SaveChangesAsync();

            var userManager = MockUserManager();
            var controller = new GamesController(db, userManager.Object);

            var result = await controller.Index("Action", "Xbox", "Mixed", 1);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<PaginatedList<Game>>(viewResult.Model);

            Assert.Single(model);
            Assert.Equal("Mixed Game", model[0].Title);
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

            var controller = new GamesController(db, MockUserManager().Object);

            var result = await controller.Details(1);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Game>(viewResult.Model);
            Assert.Equal("Test Game", model.Title);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenGameMissing()
        {
            var db = GetInMemoryDb();
            var controller = new GamesController(db, MockUserManager().Object);

            var result = await controller.Details(999);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task PostReview_Redirects_WhenValidModel()
        {
            var db = GetInMemoryDb();
            db.Games.Add(new Game
            {
                Id = 1,
                Title = "Review Test",
                Description = "Nice Game",
                Genre = new Genre { Name = "Shooter" },
                Platform = new Platform { Name = "PC" }
            });
            db.SaveChanges();

            var user = new ApplicationUser { Id = "user1", UserName = "testuser" };
            var userManager = MockUserManager();
            userManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            var controller = new GamesController(db, userManager.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, "user1"),
                        new Claim(ClaimTypes.Name, "testuser")
                    }, "TestAuth"))
                }
            };

            var reviewModel = new ReviewViewModel
            {
                GameId = 1,
                Rating = 8,
                Content = "Very fun!"
            };

            var result = await controller.PostReview(reviewModel);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Details", redirect.ActionName);
            Assert.Equal(1, redirect.RouteValues["id"]);
            Assert.Single(db.Reviews.ToList());
        }

        [Fact]
        public async Task PostReview_Redirects_WhenModelInvalid()
        {
            var db = GetInMemoryDb();
            var userManager = MockUserManager();
            var controller = new GamesController(db, userManager.Object);

            controller.ModelState.AddModelError("Rating", "Required");

            var result = await controller.PostReview(new ReviewViewModel { GameId = 1 });
            var redirect = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Details", redirect.ActionName);
        }
    }
}



