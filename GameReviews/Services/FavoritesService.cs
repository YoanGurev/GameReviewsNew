using GameReviews.Data;
using GameReviews.Models;
using GameReviews.Models.ViewModels;
using GameReviews.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameReviews.Services
{
    public class FavoritesService : IFavoritesService
    {
        private readonly ApplicationDbContext _context;

        public FavoritesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<FavoritesViewModel>> GetUserFavoritesAsync(string userId, int page, int pageSize)
        {
            var query = _context.Favorites
                .Where(f => f.UserId == userId)
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
                });

            return await PaginatedList<FavoritesViewModel>.CreateAsync(query, page, pageSize);
        }

        public async Task ToggleFavoriteAsync(string userId, int gameId)
        {
            var existing = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.GameId == gameId);

            if (existing != null)
            {
                _context.Favorites.Remove(existing);
            }
            else
            {
                var favorite = new Favorite
                {
                    UserId = userId,
                    GameId = gameId
                };
                _context.Favorites.Add(favorite);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> RemoveFavoriteAsync(string userId, int gameId)
        {
            var favorite = await _context.Favorites
                .Include(f => f.Game)
                .FirstOrDefaultAsync(f => f.UserId == userId && f.GameId == gameId);

            if (favorite == null)
                return false;

            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

