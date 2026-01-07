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

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var project = await _context.Projects
                .Include(p => p.Owner)
                .Include(p => p.Members)
                    .ThenInclude(pm => pm.User) 
                .FirstOrDefaultAsync(m => m.Id == id);

            if (project == null) return NotFound();

            bool isUserLoggedIn = User.Identity?.IsAuthenticated ?? false;

            if (!isUserLoggedIn)
            {
                
                project.Members = project.Members.Where(m => !m.User.IsPrivate).ToList();
            }

            return View(project);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Join(int projectId)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return Challenge();

            
            var alreadyMember = await _context.ProjectMembers
                .AnyAsync(pm => pm.ProjectId == projectId && pm.UserId == userId);

            if (!alreadyMember)
            {
                var membership = new ProjectMember
                {
                    ProjectId = projectId,
                    UserId = userId
                };
                _context.ProjectMembers.Add(membership);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), new { id = projectId });
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var projects = await _context.Projects
                .Include(p => p.Owner)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
            return View(projects);
        }

        
        public IActionResult Create() => View();

        
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


        
        public async Task<IActionResult> Edit(int id)
        {
           
            var project = await _context.Projects.FindAsync(id);

            
            if (project == null || project.OwnerId != _userManager.GetUserId(User))
            {
                return Forbid();
            }

            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Project project)
        {
            var currentUserId = _userManager.GetUserId(User);
            if (id != project.Id || project.OwnerId != currentUserId)
            {
                return Forbid();
            }


            ModelState.Remove("Owner");
            ModelState.Remove("Members");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(project);
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}
