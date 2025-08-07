using GameReviews.Models.ViewModels;
using System.Threading.Tasks;

namespace GameReviews.Services.Interfaces
{
    public interface IContactService
    {
        Task SubmitContactFormAsync(ContactFormViewModel model);
    }
}

