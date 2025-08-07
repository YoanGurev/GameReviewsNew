using GameReviews.Helpers;
using GameReviews.Models;
using GameReviews.Models.ViewModels;

namespace GameReviews.Services.Interfaces
{
    public interface IGameService
    {
        Task<PaginatedList<Game>> GetFilteredGamesAsync(string? genre, string? platform, string? search, int page, int pageSize);

        Task<List<string>> GetAllGenresAsync();

        Task<List<string>> GetAllPlatformsAsync();

        Task<Game?> GetGameDetailsAsync(int id);

        Task<double> CalculateAverageRatingAsync(Game game);

        Task PostReviewAsync(ReviewViewModel model, string userId);
        Task<int> UpvoteReviewAsync(int reviewId, string userId, bool isUpvote);
        Task<int> DownvoteReviewAsync(int reviewId, string userId, bool isUpvote);
        

    }
}


