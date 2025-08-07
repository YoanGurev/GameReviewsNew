using GameReviews.Services.Interfaces;
using GameReviews.Models.ViewModels;
using GameReviews.Services.Interfaces;
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

        public IActionResult Index() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(ContactFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);

            await _contactService.SubmitContactFormAsync(model);

            TempData["SuccessMessage"] = "Your message has been sent successfully!";
            return RedirectToAction("Index");
        }
    }
}



