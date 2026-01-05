using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CvSite.Web.Data;
using CvSite.Web.Data.Entities;

namespace CvSite.Web.Controllers
{
    [Authorize]
    public class EducationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EducationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var Educations = await _context.Educations
                .Where(c => c.ApplicationUserId == userId)
                .ToListAsync();

            return View(Educations);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Institution,Subject,StartDate,EndDate")] Education education)
        {
            var currentUserId = _userManager.GetUserId(User);
            if (currentUserId == null) return Challenge();

            education.ApplicationUserId = currentUserId;

            
            ModelState.Remove("ApplicationUserId");
            ModelState.Remove("ApplicationUser");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(education);
                    await _context.SaveChangesAsync();
                    
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Ett tekniskt fel uppstod när utbildningen skulle sparas.");
            }
            }
            return View(education);
        }
    }
}
