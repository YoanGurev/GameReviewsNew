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
        Task<bool> DeleteReviewAsync(int reviewId, string userId, bool isAdmin);
        Task<int> UpvoteReviewAsync(int reviewId, string userId, bool isUpvote);
        Task<int> DownvoteReviewAsync(int reviewId, string userId, bool isUpvote);
        Task<int> HandleVoteAsync(int reviewId, string userId, bool isUpvote);
        Task<(int Upvotes, int Downvotes)> GetReviewVotesAsync(int reviewId);

        Task<string> GetUserVoteTypeAsync(int reviewId, string userId);

        Task<ReviewReply> AddReplyAsync(int reviewId, string userId, string content);
        Task<bool> DeleteReplyAsync(int replyId, string userId, bool isAdmin);
        Task<IEnumerable<ReviewReply>> GetRepliesAsync(int reviewId);

        Task<Review?> GetReviewDetailsAsync(int reviewId);
        Task<ReviewReply?> GetReplyDetailsAsync(int replyId);


    }
}


