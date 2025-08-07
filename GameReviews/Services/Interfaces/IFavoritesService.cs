using GameReviews.Models.ViewModels;

namespace GameReviews.Services.Interfaces
{
    public interface IFavoritesService
    {
        Task<PaginatedList<FavoritesViewModel>> GetUserFavoritesAsync(string userId, int page, int pageSize);
        Task ToggleFavoriteAsync(string userId, int gameId);
        Task<bool> RemoveFavoriteAsync(string userId, int gameId);
    }
}
