using GameReviews.Models;
using GameReviews.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameReviews.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class AdminGameRequestsController : Controller
    {
        private readonly IGameRequestService _gameRequestService;
        private readonly IAdminGameService _adminGameService;

        public AdminGameRequestsController(
            IGameRequestService gameRequestService,
            IAdminGameService adminGameService)
        {
            _gameRequestService = gameRequestService;
            _adminGameService = adminGameService;
        }

        public async Task<IActionResult> Inbox()
        {
            var requests = await _gameRequestService.GetAllRequestsWithUsersAsync();
            return View(requests);
        }

        public async Task<IActionResult> Review(int id)
        {
            var request = await _gameRequestService.GetRequestByIdAsync(id);
            if (request == null) return NotFound();
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var request = await _gameRequestService.GetRequestByIdAsync(id);
            if (request == null) return NotFound();

            
            var newGame = new Game
            {
                Title = request.Title,
                Description = request.Description,
                Price = request.Price,
                GenreId = request.GenreId,
                PlatformId = request.PlatformId,
                ImageUrl = request.ImageUrl,
                VideoUrl = request.VideoUrl,
                ReleaseDate = DateTime.UtcNow
            };

            await _adminGameService.CreateGameAsync(newGame);

            
            await _gameRequestService.UpdateRequestStatusAsync(id, "Approved", "Your request was approved.");

            return RedirectToAction("Inbox");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deny(int id, string reason)
        {
            var request = await _gameRequestService.GetRequestByIdAsync(id);
            if (request == null) return NotFound();

            await _gameRequestService.UpdateRequestStatusAsync(id, "Denied", reason);

            return RedirectToAction("Inbox");
        }


    }
}

