using GameReviews.Data;
using GameReviews.Models;
using GameReviews.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameReviews.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class AdminContactsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminContactsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Inbox(int page = 1)
        {
            int pageSize = 5;

            var query = _context.ContactForms
                .OrderByDescending(m => m.SubmittedAt);

            var messages = await PaginatedList<ContactForm>.CreateAsync(query, page, pageSize);
            return View(messages);
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var message = await _context.ContactForms.FindAsync(id);
            if (message != null && !message.IsRead)
            {
                message.IsRead = true;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Inbox");
        }
    }
}

