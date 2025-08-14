using GameReviews.Models;
using GameReviews.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [HttpGet]
        public async Task<IActionResult> Reply(int id)
        {
            var message = await _contactService.GetMessageByIdAsync(id);
            if (message == null) return NotFound();

            return View(message);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reply(int id, string replyMessage)
        {
            if (string.IsNullOrWhiteSpace(replyMessage))
            {
                TempData["ErrorMessage"] = "Reply cannot be empty.";
                return RedirectToAction("Reply", new { id });
            }

            var adminUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var success = await _contactService.ReplyToMessageAsync(id, replyMessage, adminUserId);
            if (!success) return NotFound();

            TempData["SuccessMessage"] = "Message replied successfully.";
            return RedirectToAction("Inbox");
        }




    }
}


