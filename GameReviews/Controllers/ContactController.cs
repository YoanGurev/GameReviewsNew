using GameReviews.Models;
using GameReviews.Models.ViewModels;
using GameReviews.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GameReviews.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new ContactFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(ContactFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            ApplicationUser? user = null;

            if (!string.IsNullOrEmpty(userId))
            {
                user = await _contactService.FindByIdAsync(userId);
            }

            await _contactService.SubmitContactFormAsync(model, user);

            TempData["SuccessMessage"] = "Your message has been sent successfully!";
            return RedirectToAction("Index");
        }

    }
}




