using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CvSite.Web.Data;
using CvSite.Web.Data.Entities;



namespace CvSite.Web.Controllers
{
    [Authorize]
    public class CompetencesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CompetencesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var competences = await _context.Competences
                .Where(c => c.ApplicationUserId == userId)
                .ToListAsync();

            return View(competences);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Competence competences)
        {
            if (!ModelState.IsValid)
            {
                return View(competences);
            }

            competences.ApplicationUserId = _userManager.GetUserId(User)!;

            try
            {
                _context.Add(competences);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                return View(competences);

            }
        }
    }
}
