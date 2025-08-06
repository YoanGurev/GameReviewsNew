using Microsoft.AspNetCore.Mvc;
using GameReviews.Models;

public class ContactController : Controller
{
    public IActionResult Index() => View();

    [HttpPost]
    public IActionResult Send(ContactForm model)
    {
        if (!ModelState.IsValid)
            return View("Index", model);

        TempData["SuccessMessage"] = "Your message has been sent!";
        return RedirectToAction("Index");
    }
}

