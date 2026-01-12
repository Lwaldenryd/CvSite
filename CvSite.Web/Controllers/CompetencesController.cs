using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CvSite.Data;
using CvSite.Data.Entities;



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
        public async Task<IActionResult> Create([Bind("Name")] Competence competence)
        {
            var currentUserId = _userManager.GetUserId(User);

            if (currentUserId == null) return Challenge();

            competence.ApplicationUserId = currentUserId;

            ModelState.Remove("ApplicationUserId");
            ModelState.Remove("ApplicationUser");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(competence);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("MyPage", "Cv");
                }
                catch (Exception ex)
                {
                   
                    ModelState.AddModelError("", "Ett tekniskt fel uppstod när kompetensen skulle sparas.");
                }
            }
            return View(competence);
        }
    }
}
