using GameReviews.Models;
using GameReviews.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameReviews.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class AdminContactsController : Controller
    {
        private readonly IAdminContactService _contactService;

        public AdminContactsController(IAdminContactService contactService)
        {
            _contactService = contactService;
        }

        public async Task<IActionResult> Inbox(int page = 1)
        {
            int pageSize = 5;
            var messages = await _contactService.GetInboxAsync(page, pageSize);
            return View(messages);
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await _contactService.MarkAsReadAsync(id);
            return RedirectToAction("Inbox");
        }
    }
}


