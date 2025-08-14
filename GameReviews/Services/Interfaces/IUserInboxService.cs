using GameReviews.Models.ViewModels;
using System.Threading.Tasks;

namespace GameReviews.Services.Interfaces
{
    public interface IUserInboxService
    {
        Task<UserInboxViewModel> GetInboxForUserAsync(string userId);
    }
}

