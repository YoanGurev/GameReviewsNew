using GameReviews.Data;
using GameReviews.Models;
using GameReviews.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameReviews.Controllers
{
    public class GamesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GamesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Games
        public async Task<IActionResult> Index(string? genre, string? platform, string? search, int page = 1)
        {
            var query = _context.Games
                .Include(g => g.Genre)
                .Include(g => g.Platform)
                .Include(g => g.Reviews) 
                .AsQueryable();

            if (!string.IsNullOrEmpty(genre))
                query = query.Where(g => g.Genre.Name == genre);

            if (!string.IsNullOrEmpty(platform))
                query = query.Where(g => g.Platform.Name == platform);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(g => g.Title.Contains(search));

            ViewBag.Genres = await _context.Genres.Select(g => g.Name).ToListAsync();
            ViewBag.Platforms = await _context.Platforms.Select(p => p.Name).ToListAsync();
            ViewBag.Search = search;

            int pageSize = 6;
            var paginatedGames = await PaginatedList<Game>.CreateAsync(query, page, pageSize);

            return View(paginatedGames);
        }

        // GET: /Games/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var game = await _context.Games
                .Include(g => g.Genre)
                .Include(g => g.Platform)
                .Include(g => g.Reviews)
                    .ThenInclude(r => r.User)
                .Include(g => g.Favorites)
                    .ThenInclude(f => f.User)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null) return NotFound();

            // Calculate average rating
            ViewBag.AverageRating = game.Reviews.Any()
                ? Math.Round(game.Reviews.Average(r => r.Rating), 1)
                : 0.0;

            return View(game);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> PostReview(ReviewViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Details", new { id = model.GameId });
            }

            var user = await _userManager.GetUserAsync(User);
            var review = new Review
            {
                GameId = model.GameId,
                UserId = user.Id,
                Rating = model.Rating,
                Content = model.Content,
                CreatedAt = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = model.GameId });
        }
    }
}



