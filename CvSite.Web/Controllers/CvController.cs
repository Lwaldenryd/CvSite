using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CvSite.Web.Data;
using CvSite.Web.Data.Entities;
using CvSite.Web.Models;

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
                usersQuery = usersQuery.Where(u => !u.IsPrivate);
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
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound();

            if (user.IsPrivate && !User.Identity!.IsAuthenticated)
            {
                return Challenge(); 
            }

            var viewModel = new CvViewModel
            {
                User = user,
                Competences = user.Competences.ToList(),
                Educations = user.Educations.ToList(),
                Experiences = user.Experiences.ToList()
            };

            return View(viewModel);
        }
    }
}
