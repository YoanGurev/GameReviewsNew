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
            db.Games.AddRange(new List<Game>
            {
                new Game { Title = "Game A" },
                new Game { Title = "Game B" },
                new Game { Title = "Game C" }
            });
            db.SaveChanges();

            var userManager = MockUserManager();
            var controller = new GamesController(db, userManager.Object);

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
                GenreId = genre.Id,
                PlatformId = platform.Id
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
            var game = new Game { Id = 1, Title = "Review Test" };
            db.Games.Add(game);
            db.SaveChanges();

            var user = new ApplicationUser { Id = "user1", UserName = "testuser" };
            var userManager = MockUserManager();
            userManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            var controller = new GamesController(db, userManager.Object);
            var httpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "user1"),
                    new Claim(ClaimTypes.Name, "testuser")
                }, "TestAuth"))
            };
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
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

