using GameReviews.Models.ViewModels;
using GameReviews.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GameReviews.Controllers
{
    [Authorize]
    public class UserInboxController : Controller
    {
        private readonly IUserInboxService _inboxService;

        public UserInboxController(IUserInboxService inboxService)
        {
            _inboxService = inboxService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var model = await _inboxService.GetInboxForUserAsync(userId);
            return View(model);
        }
    }
}

