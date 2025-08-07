using GameReviews.Data;
using GameReviews.Helpers;
using GameReviews.Models;
using GameReviews.Models.ViewModels;
using GameReviews.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GameReviews.Services
{
    public class GameService : IGameService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GameService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PaginatedList<Game>> GetFilteredGamesAsync(string? genre, string? platform, string? search, int page, int pageSize)
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

            return await PaginatedList<Game>.CreateAsync(query, page, pageSize);
        }

        public async Task<List<string>> GetAllGenresAsync()
        {
            return await _context.Genres.Select(g => g.Name).ToListAsync();
        }

        public async Task<List<string>> GetAllPlatformsAsync()
        {
            return await _context.Platforms.Select(p => p.Name).ToListAsync();
        }

        public async Task<Game?> GetGameDetailsAsync(int id)
        {
            return await _context.Games
                .Include(g => g.Genre)
                .Include(g => g.Platform)
                .Include(g => g.Reviews)
                    .ThenInclude(r => r.User)
                .Include(g => g.Favorites)
                    .ThenInclude(f => f.User)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<double> CalculateAverageRatingAsync(Game game)
        {
            return game.Reviews.Any()
                ? Math.Round(game.Reviews.Average(r => r.Rating), 1)
                : 0.0;
        }

        public async Task PostReviewAsync(ReviewViewModel model, string userId)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

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
        }

        public async Task<int> UpvoteReviewAsync(int reviewId, string userId, bool isUpvote)
        {
            return await HandleVoteAsync(reviewId, userId, isUpvote: true);
        }

        public async Task<int> DownvoteReviewAsync(int reviewId, string userId, bool isUpvote)
        {
            return await HandleVoteAsync(reviewId, userId, isUpvote: false);
        }

        private async Task<int> HandleVoteAsync(int reviewId, string userId, bool isUpvote)
        {
            var review = await _context.Reviews.FindAsync(reviewId);
            if (review == null) throw new Exception("Review not found");

            var existingVote = await _context.ReviewVotes
                .FirstOrDefaultAsync(v => v.ReviewId == reviewId && v.UserId == userId);

            if (existingVote != null)
            {
                if (existingVote.IsUpvote == isUpvote)
                {
                    // Same vote - undo
                    _context.ReviewVotes.Remove(existingVote);
                    if (isUpvote) review.Upvotes--;
                    else review.Downvotes--;
                }
                else
                {
                    // Change vote direction
                    existingVote.IsUpvote = isUpvote;
                    if (isUpvote)
                    {
                        review.Upvotes++;
                        review.Downvotes--;
                    }
                    else
                    {
                        review.Upvotes--;
                        review.Downvotes++;
                    }
                }
            }
            else
            {
                // First vote
                var vote = new ReviewVote
                {
                    ReviewId = reviewId,
                    UserId = userId,
                    IsUpvote = isUpvote
                };
                _context.ReviewVotes.Add(vote);

                if (isUpvote) review.Upvotes++;
                else review.Downvotes++;
            }

            await _context.SaveChangesAsync();
            return review.GameId;
        }







    }
}


