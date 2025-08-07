using GameReviews.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameReviews.Services
{
    public interface IAdminGameService
    {
        Task<List<Game>> GetAllGamesAsync();
        Task<Game?> GetGameByIdAsync(int id);
        Task<bool> CreateGameAsync(Game game);
        Task<bool> UpdateGameAsync(Game game);
        Task<bool> DeleteGameAsync(int id);
        Task<Dictionary<string, SelectList>> GetDropdownsAsync();
    }
}

