using GameReviews.Data;
using GameReviews.Services.Interfaces;
using GameReviews.Models;
using GameReviews.Models.ViewModels;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace GameReviews.Services
{
    public class ContactService : IContactService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ContactService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<ApplicationUser?> FindByIdAsync(string userId)
        {
            
            return await _context.Users.FindAsync(userId);
            
        }

        public async Task SubmitContactFormAsync(ContactFormViewModel model, ApplicationUser? user)
        {
            var entity = new ContactForm
            {
                Subject = model.Subject,
                Message = model.Message,
                SubmittedByUserId = user?.Id,
                Name = user?.UserName ?? model.Name,
                Email = user?.Email ?? model.Email,
                SubmittedAt = DateTime.UtcNow,
                IsRead = false
            };

            _context.ContactForms.Add(entity);
            await _context.SaveChangesAsync();
        }



    }
}



