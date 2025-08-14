using GameReviews.Models;
using GameReviews.Models.ViewModels;
using System.Threading.Tasks;

namespace GameReviews.Services.Interfaces
{
    public interface IContactService
    {
        Task<ApplicationUser?> FindByIdAsync(string userId);
        Task SubmitContactFormAsync(ContactFormViewModel model, ApplicationUser? user);

    }
}

