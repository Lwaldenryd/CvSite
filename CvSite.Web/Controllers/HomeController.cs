using System.Diagnostics;
using CvSite.Web.Data;
using CvSite.Web.Data.Entities;
using CvSite.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CvSite.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // 1. Kolla om någon är inloggad
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                // 2. Om inloggad: Skicka användaren direkt till CvController och Details-metoden
                return RedirectToAction("Details", "Cv", new { id = user.Id });
            }

            // 3. Om INTE inloggad: Visa bara den vanliga välkomstsidan (Home/Index.cshtml)
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
    }
}
