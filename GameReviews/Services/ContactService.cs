using GameReviews.Data;
using GameReviews.Services.Interfaces;
using GameReviews.Models;
using GameReviews.Models.ViewModels;
using System;
using System.Threading.Tasks;

namespace GameReviews.Services
{
    public class ContactService : IContactService
    {
        private readonly ApplicationDbContext _context;

        public ContactService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SubmitContactFormAsync(ContactFormViewModel model)
        {
            var entity = new ContactForm
            {
                Name = model.Name,
                Email = model.Email,
                Subject = model.Subject,
                Message = model.Message,
                SubmittedAt = DateTime.UtcNow
            };

            _context.ContactForms.Add(entity);
            await _context.SaveChangesAsync();
        }
    }
}


