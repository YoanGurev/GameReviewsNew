using System.Diagnostics;
using GameReviews.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameReviews.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("Home/Error404")]
        public IActionResult Error404()
        {
            return View("Error404");
        }

        [Route("Home/Error500")]
        public IActionResult Error500()
        {
            return View("Error500");
        }

        [Route("Home/Error{code:int}")]
        public IActionResult Error(int code)
        {
            if (code == 404)
                return RedirectToAction("Error404");
            if (code == 500)
                return RedirectToAction("Error500");

            
            return View("Error", code);
        }

    }
}
