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
        public async Task<ContactForm?> GetMessageByIdAsync(int id)
        {
            return await _context.ContactForms.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<bool> ReplyToMessageAsync(int id, string replyMessage, string adminUserId)
        {
            var message = await _context.ContactForms.FindAsync(id);
            if (message == null) return false;

            message.ReplyMessage = replyMessage;
            message.RepliedByUserId = adminUserId;
            message.RepliedAt = DateTime.UtcNow;
            message.IsRead = true;

            await _context.SaveChangesAsync();
            return true;
        }


    }
}
