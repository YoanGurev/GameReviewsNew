using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GameReviews.Data;
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

        public async Task<IActionResult> Inbox()
        {
            var messages = await _context.ContactForms
                .OrderByDescending(m => m.SubmittedAt)
                .ToListAsync();

            return View(messages);
        }
    }
}

