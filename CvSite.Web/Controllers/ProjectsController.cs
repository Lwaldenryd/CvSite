using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CvSite.Web.Data;
using CvSite.Web.Data.Entities;

namespace CvSite.Web.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProjectsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Requirement 10: List all projects
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var projects = await _context.Projects
                .Include(p => p.Owner)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
            return View(projects);
        }

        // Requirement 8: Create Project (GET)
        public IActionResult Create() => View();

        // Requirement 8: Create Project (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Project project)
        {
            project.OwnerId = _userManager.GetUserId(User);
            project.CreatedAt = DateTime.Now;

            ModelState.Remove("OwnerId");
            ModelState.Remove("Owner");

            if (ModelState.IsValid)
            {
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        // Requirement 9: Edit own projects
        public async Task<IActionResult> Edit(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null || project.OwnerId != _userManager.GetUserId(User))
                return Forbid();

            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Project project)
        {
            if (project.OwnerId != _userManager.GetUserId(User)) return Forbid();

            if (ModelState.IsValid)
            {
                _context.Update(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }
    }
}
