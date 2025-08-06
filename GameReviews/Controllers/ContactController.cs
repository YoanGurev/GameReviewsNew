using Microsoft.AspNetCore.Mvc;
using GameReviews.Data;
using GameReviews.Models;
using GameReviews.Models.ViewModels;
public class ContactController : Controller
{
    private readonly ApplicationDbContext _context;

    public ContactController(ApplicationDbContext context)
    {
        _context = context;
    }
    public IActionResult Index() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit(ContactFormViewModel model)
    {
        if (!ModelState.IsValid)
            return View("Index", model);

        var entity = new ContactForm
        {
            Name = model.Name,
            Email = model.Email,
            Subject = model.Subject,
            Message = model.Message,
            SubmittedAt = DateTime.UtcNow
        };

        _context.ContactForms.Add(entity);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Your message has been sent successfully!";

        return RedirectToAction("Index");
    }
}

