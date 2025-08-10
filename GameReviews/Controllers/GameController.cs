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
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            var userId = _userManager.GetUserId(User);
            bool isAdmin = User.IsInRole("Administrator");

            var result = await _gameService.DeleteReviewAsync(reviewId, userId, isAdmin);
            if (!result) return Forbid();

            // Redirect back to the game details page
            var review = await _gameService.GetReviewDetailsAsync(reviewId);
            if (review == null)
                return RedirectToAction("Index");

            return RedirectToAction("Details", new { id = review.GameId });
        }



        public async Task<IActionResult> Upvote(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var gameId = await _gameService.HandleVoteAsync(id, user.Id, true);
            var votes = await _gameService.GetReviewVotesAsync(id);
            var userVote = await _gameService.GetUserVoteTypeAsync(id, user.Id);

            return Json(new { upvotes = votes.Upvotes, downvotes = votes.Downvotes, userVote });
        }

        public async Task<IActionResult> Downvote(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var gameId = await _gameService.HandleVoteAsync(id, user.Id, false);
            var votes = await _gameService.GetReviewVotesAsync(id);
            var userVote = await _gameService.GetUserVoteTypeAsync(id, user.Id);

            return Json(new { upvotes = votes.Upvotes, downvotes = votes.Downvotes, userVote });
        }




        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostReply(int reviewId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return BadRequest("Reply content cannot be empty.");

            var userId = _userManager.GetUserId(User);
            var reply = await _gameService.AddReplyAsync(reviewId, userId, content);

            if (reply == null) return NotFound();

            return PartialView("_ReplyPartial", reply);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteReply(int replyId)
        {
            var userId = _userManager.GetUserId(User);
            bool isAdmin = User.IsInRole("Administrator");

            var result = await _gameService.DeleteReplyAsync(replyId, userId, isAdmin);
            if (!result) return Forbid();

            // Redirect back to the game details page
            var reply = await _gameService.GetReplyDetailsAsync(replyId);
            if (reply == null)
                return RedirectToAction("Index");

            return RedirectToAction("Details", new { id = reply.Review.GameId });
        }


        [HttpGet]
        public async Task<IActionResult> GetReplies(int reviewId)
        {
            var replies = await _gameService.GetRepliesAsync(reviewId);
            return PartialView("_RepliesListPartial", replies);
        }



    }
}




