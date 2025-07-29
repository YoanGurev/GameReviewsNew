using GameReviews.Data;
using GameReviews.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GameReviews.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        //GET Admin
        public async Task<IActionResult> Index()
        {
            var games = await _context.Games
                .Include(g => g.Genre)
                .Include(g => g.Platform)
                .ToListAsync();

            return View(games);
        }

        //GET AdminCreate
        public async Task<IActionResult> Create()
        {
            await LoadDropdownsAsync();
            return View();
        }

        //POST Admin Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Game game)
        {
            if (ModelState.IsValid)
            {
                _context.Games.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await LoadDropdownsAsync();
            return View(game);
        }

        //GET Admin Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null) return NotFound();

            await LoadDropdownsAsync();
            return View(game);
        }

        //POST Admin Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Game game)
        {
            if (id != game.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await LoadDropdownsAsync();
            return View(game);
        }

        //GET Admin Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var game = await _context.Games
                .Include(g => g.Genre)
                .Include(g => g.Platform)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null) return NotFound();

            return View(game);
        }

        //POST Admin Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game != null)
            {
                _context.Games.Remove(game);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Helper method to populate dropdowns
        private async Task LoadDropdownsAsync()
        {
            ViewBag.Genres = new SelectList(await _context.Genres.ToListAsync(), "Id", "Name");
            ViewBag.Platforms = new SelectList(await _context.Platforms.ToListAsync(), "Id", "Name");
        }
    }
}

