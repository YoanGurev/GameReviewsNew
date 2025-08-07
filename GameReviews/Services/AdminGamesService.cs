using GameReviews.Data;
using GameReviews.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GameReviews.Services
{
    public class AdminGameService : IAdminGameService
    {
        private readonly ApplicationDbContext _context;

        public AdminGameService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Game>> GetAllGamesAsync()
        {
            return await _context.Games
                .Include(g => g.Genre)
                .Include(g => g.Platform)
                .ToListAsync();
        }

        public async Task<Game?> GetGameByIdAsync(int id)
        {
            return await _context.Games.FindAsync(id);
        }

        public async Task<bool> CreateGameAsync(Game game)
        {
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateGameAsync(Game game)
        {
            _context.Update(game);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteGameAsync(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null) return false;

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Dictionary<string, SelectList>> GetDropdownsAsync()
        {
            var genres = await _context.Genres.ToListAsync();
            var platforms = await _context.Platforms.ToListAsync();

            return new Dictionary<string, SelectList>
            {
                { "Genres", new SelectList(genres, "Id", "Name") },
                { "Platforms", new SelectList(platforms, "Id", "Name") }
            };
        }
    }
}

