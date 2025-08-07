using GameReviews.Models;
using GameReviews.Models.ViewModels;
using System.Threading.Tasks;

namespace GameReviews.Services
{
    public interface IAdminContactService
    {
        Task<PaginatedList<ContactForm>> GetInboxAsync(int page, int pageSize);
        Task<bool> MarkAsReadAsync(int id);
    }
}

