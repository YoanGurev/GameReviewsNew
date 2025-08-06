using GameReviews.Data;
using GameReviews.Models;
using GameReviews.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameReviews.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FavoritesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var favorites = await _context.Favorites
                .Where(f => f.UserId == user.Id)
                .Include(f => f.Game)
                    .ThenInclude(g => g.Genre)
                .Include(f => f.Game.Platform)
                 .Select(f => new FavoritesViewModel
                 {
                     GameId = f.Game.Id,
                     Title = f.Game.Title,
                     GenreName = f.Game.Genre.Name,
                     PlatformName = f.Game.Platform.Name,
                     ImageUrl = f.Game.ImageUrl,
                     Price = f.Game.Price
                 })
        .ToListAsync();

            return View(favorites);
        }

        [HttpPost]
        public async Task<IActionResult> Toggle(int gameId)
        {
            var user = await _userManager.GetUserAsync(User);
            var existing = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == user.Id && f.GameId == gameId);

            if (existing != null)
            {
                _context.Favorites.Remove(existing);
            }
            else
            {
                var newFavorite = new Favorite
                {
                    UserId = user.Id,
                    GameId = gameId
                };
                _context.Favorites.Add(newFavorite);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Games", new { id = gameId });
        }
    }
}

