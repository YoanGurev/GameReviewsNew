using GameReviews.Data;
using GameReviews.Models;
using GameReviews.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GameReviews.Services
{
    public class AdminContactService : IAdminContactService
    {
        private readonly ApplicationDbContext _context;

        public AdminContactService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<ContactForm>> GetInboxAsync(int page, int pageSize)
        {
            var query = _context.ContactForms
                .OrderByDescending(m => m.SubmittedAt);

            return await PaginatedList<ContactForm>.CreateAsync(query, page, pageSize);
        }

        public async Task<bool> MarkAsReadAsync(int id)
        {
            var message = await _context.ContactForms.FindAsync(id);
            if (message == null || message.IsRead) return false;

            message.IsRead = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
