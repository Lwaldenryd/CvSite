using CvSite.Data;
using CvSite.Data.Entities;
using CvSite.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CvSite.Web.Controllers
{
    public class CvController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CvController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        
        public async Task<IActionResult> Index(string searchString)
        {
            var usersQuery = _userManager.Users
                .Include(u => u.Competences)
                .AsQueryable();

            bool isUserLoggedIn = User.Identity?.IsAuthenticated ?? false;

            if (!isUserLoggedIn)
            {
                usersQuery = usersQuery.Where(u => u.IsPrivate == false);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                usersQuery = usersQuery.Where(u =>
                    (u.FirstName + " " + u.LastName).Contains(searchString) ||
                    u.Competences.Any(c => c.Name.Contains(searchString)));
            }

            var users = await usersQuery.ToListAsync();
            return View(users);
        }
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var user = await _userManager.Users
                .Include(u => u.Competences)
                .Include(u => u.Educations)
                .Include(u => u.Experiences)
                //.Include(u => u.ProjectMembers)
                //.ThenInclude(pm => pm.Project)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound();

            var members = await _context.ProjectMembers
                 .Include(pm => pm.Project)
                 .Where(pm => pm.UserId == id)
                 .ToListAsync();

            if (user.IsPrivate && !User.Identity!.IsAuthenticated)
            {
                return Challenge(); 
            }

            var viewModel = new CvViewModel
            {
                User = user,
                Competences = user.Competences.ToList(),
                Educations = user.Educations.ToList(),
                Experiences = user.Experiences.ToList(),
                ProjectMembers = members
            };

            return View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> MyPage()
        {
            
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            var detailsResult = await Details(userId);

            if (detailsResult is ViewResult viewResult)
            {
                
                viewResult.ViewName = "Details";
                return viewResult;
            }

            return detailsResult;
        }
    }
}
