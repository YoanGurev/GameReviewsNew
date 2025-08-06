using GameReviews.Data;
using GameReviews.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GameReviews.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class AdminGamesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminGamesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AdminGames
        public async Task<IActionResult> Index()
        {
            var games = await _context.Games
                .Include(g => g.Genre)
                .Include(g => g.Platform)
                .ToListAsync();

            return View(games);
        }

        // GET: AdminGames/Create
        public async Task<IActionResult> Create()
        {
            await LoadDropdownsAsync();
            return View();
        }

        // POST: AdminGames/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Game game)
        {
            Console.WriteLine($"GenreId: {game.GenreId}, PlatformId: {game.PlatformId}");
            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    foreach (var error in entry.Value.Errors)
                    {
                        Console.WriteLine($"Error for {entry.Key}: {error.ErrorMessage}");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                _context.Games.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await LoadDropdownsAsync();
            return View(game);
        }


        // GET: AdminGames/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null) return NotFound();

            await LoadDropdownsAsync();
            return View(game);
        }

        // POST: AdminGames/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Game game)
        {
            if (id != game.Id) return NotFound();

            if (!await _context.Games.AnyAsync(g => g.Id == id))
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Game updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Failed to update game. Try again.");
                }
            }

            await LoadDropdownsAsync();
            return View(game);
        }

        // GET: AdminGames/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var game = await _context.Games
                .Include(g => g.Genre)
                .Include(g => g.Platform)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null) return NotFound();

            return View(game);
        }

        // POST: AdminGames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null) return NotFound();

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Game deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
        

        // Helper: load dropdowns for Genre and Platform
        private async Task LoadDropdownsAsync()
        {
            ViewBag.Genres = new SelectList(await _context.Genres.ToListAsync(), "Id", "Name");
            ViewBag.Platforms = new SelectList(await _context.Platforms.ToListAsync(), "Id", "Name");
        }
    }
}


