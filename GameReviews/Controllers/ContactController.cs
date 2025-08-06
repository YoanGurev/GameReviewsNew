using Microsoft.AspNetCore.Mvc;
using GameReviews.Models.ViewModels;

public class ContactController : Controller
{
    public IActionResult Index() => View();

    [HttpPost]
    public IActionResult Send(ContactFormViewModel model)
    {
        if (!ModelState.IsValid)
            return View("Index", model);

        TempData["SuccessMessage"] = "Your message has been sent!";
        return RedirectToAction("Index");
    }
}

