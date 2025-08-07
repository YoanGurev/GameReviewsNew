using GameReviews.Models;
using GameReviews.Models.ViewModels;
using GameReviews.Services;
using GameReviews.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GameReviews.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {
        private readonly IFavoritesService _favoriteService;
        private readonly UserManager<ApplicationUser> _userManager;

        public FavoritesController(IFavoritesService favoriteService, UserManager<ApplicationUser> userManager)
        {
            _favoriteService = favoriteService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 6)
        {
            var user = await _userManager.GetUserAsync(User);
            var favorites = await _favoriteService.GetUserFavoritesAsync(user.Id, page, pageSize);
            return View(favorites);
        }

        [HttpPost]
        public async Task<IActionResult> Toggle(int gameId)
        {
            var user = await _userManager.GetUserAsync(User);
            await _favoriteService.ToggleFavoriteAsync(user.Id, gameId);
            return RedirectToAction("Details", "Games", new { id = gameId });
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var success = await _favoriteService.RemoveFavoriteAsync(user.Id, id);

            if (success)
            {
                TempData["SuccessMessage"] = "You have successfully removed the game from your favorites.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}


