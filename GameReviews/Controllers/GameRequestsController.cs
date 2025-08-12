using GameReviews.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Authorize]
public class GameRequestsController : Controller
{
    private readonly IGameRequestService _requestService;
    private readonly UserManager<ApplicationUser> _userManager;

    public GameRequestsController(IGameRequestService requestService, UserManager<ApplicationUser> userManager)
    {
        _requestService = requestService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Submit()
    {
        await LoadDropdownsAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit(GameRequest gameRequest)
    {
        if (!ModelState.IsValid)
        {
            await LoadDropdownsAsync();
            return View(gameRequest);
        }

        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        gameRequest.RequestedByUserId = userId;
        gameRequest.Status = "Pending";


        await _requestService.SubmitRequestAsync(gameRequest);

        TempData["SuccessMessage"] = $"Your request for '{gameRequest.Title}' has been submitted.";
        return RedirectToAction("Index", "Home");
    }


    private async Task LoadDropdownsAsync()
    {
        var dropdowns = await _requestService.GetDropdownsAsync();
        ViewBag.Genres = dropdowns["Genres"];
        ViewBag.Platforms = dropdowns["Platforms"];
    }
}

