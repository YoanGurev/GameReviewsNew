using GameReviews.Data;
using GameReviews.Models;
using GameReviews.Models.ViewModels;
using GameReviews.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace GameReviews.Services
{
    public class UserInboxService : IUserInboxService
    {
        private readonly ApplicationDbContext _context;

        public UserInboxService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserInboxViewModel> GetInboxForUserAsync(string userId)
        {
            var contactMessages = await _context.ContactForms
                .Where(c => c.SubmittedByUserId == userId)
                .OrderByDescending(c => c.SubmittedAt)
                .ToListAsync();


            var gameRequests = await _context.GameRequests
                .Where(g => g.RequestedByUserId == userId)
                .Include(g => g.Genre)
                .Include(g => g.Platform)
                .OrderByDescending(g => g.Id)
                .ToListAsync();

            return new UserInboxViewModel
            {
                ContactMessages = contactMessages,
                GameRequests = gameRequests
            };
        }

    }
}

