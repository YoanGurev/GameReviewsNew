using GameReviews.Models;
using GameReviews.Models.ViewModels;
using System.Threading.Tasks;

namespace GameReviews.Services
{
    public interface IAdminContactService
    {
        Task<PaginatedList<ContactForm>> GetInboxAsync(int page, int pageSize);
        Task<bool> ReplyToMessageAsync(int id, string reply, string adminId);
        Task<ContactForm?> GetMessageByIdAsync (int id);
    }
}

