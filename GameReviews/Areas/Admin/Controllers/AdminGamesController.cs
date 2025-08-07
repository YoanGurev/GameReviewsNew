using GameReviews.Models;
using GameReviews.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameReviews.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class AdminGamesController : Controller
    {
        private readonly IAdminGameService _gameService;

        public AdminGamesController(IAdminGameService gameService)
        {
            _gameService = gameService;
        }

        public async Task<IActionResult> Index()
        {
            var games = await _gameService.GetAllGamesAsync();
            return View(games);
        }

        public async Task<IActionResult> Create()
        {
            await LoadDropdownsAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Game game)
        {
            if (ModelState.IsValid)
            {
                await _gameService.CreateGameAsync(game);
                return RedirectToAction(nameof(Index));
            }

            await LoadDropdownsAsync();
            return View(game);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var game = await _gameService.GetGameByIdAsync(id);
            if (game == null) return NotFound();

            await LoadDropdownsAsync();
            return View(game);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Game game)
        {
            if (id != game.Id) return NotFound();

            if (ModelState.IsValid)
            {
                await _gameService.UpdateGameAsync(game);
                TempData["SuccessMessage"] = "Game updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            await LoadDropdownsAsync();
            return View(game);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var game = await _gameService.GetGameByIdAsync(id);
            if (game == null) return NotFound();

            return View(game);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _gameService.DeleteGameAsync(id);
            TempData["SuccessMessage"] = "Game deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDropdownsAsync()
        {
            var dropdowns = await _gameService.GetDropdownsAsync();
            ViewBag.Genres = dropdowns["Genres"];
            ViewBag.Platforms = dropdowns["Platforms"];
        }
    }
}



