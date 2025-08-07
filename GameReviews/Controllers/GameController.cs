using GameReviews.Helpers;
using GameReviews.Models;
using GameReviews.Models.ViewModels;
using GameReviews.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GameReviews.Controllers
{
    public class GamesController : Controller
    {
        private readonly IGameService _gameService;
        private readonly UserManager<ApplicationUser> _userManager;

        public GamesController(IGameService gameService, UserManager<ApplicationUser> userManager)
        {
            _gameService = gameService;
            _userManager = userManager;
        }

        // GET: /Games
        public async Task<IActionResult> Index(string? genre, string? platform, string? search, int page = 1)
        {
            int pageSize = 6;

            var paginatedGames = await _gameService.GetFilteredGamesAsync(genre, platform, search, page, pageSize);
            var genres = await _gameService.GetAllGenresAsync();
            var platforms = await _gameService.GetAllPlatformsAsync();

            ViewBag.Genres = genres;
            ViewBag.Platforms = platforms;
            ViewBag.Search = search;

            return View(paginatedGames);
        }

        // GET: /Games/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var game = await _gameService.GetGameDetailsAsync(id);
            if (game == null) return NotFound();

            ViewBag.AverageRating = game.Reviews.Any()
                ? Math.Round(game.Reviews.Average(r => r.Rating), 1)
                : 0.0;

            return View(game);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> PostReview(ReviewViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Details", new { id = model.GameId });
            }

            var user = await _userManager.GetUserAsync(User);
            await _gameService.PostReviewAsync(model, user.Id);

            return RedirectToAction("Details", new { id = model.GameId });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Upvote(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var gameId = await _gameService.UpvoteReviewAsync(id, user.Id, true);
            return RedirectToAction("Details", new { id = gameId });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Downvote(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var gameId = await _gameService.DownvoteReviewAsync(id, user.Id, false);
            return RedirectToAction("Details", new { id = gameId });
        }
    }
}




