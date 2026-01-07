using System.Diagnostics;
using CvSite.Web.Data;
using CvSite.Web.Data.Entities;
using CvSite.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CvSite.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users
                .Where(u => !u.IsPrivate)
                .OrderByDescending(u => u.Id)
                .Take(4)
                .ToListAsync();

            var latestProject = await _context.Projects
                .Include(p => p.Owner)
                .OrderByDescending(p => p.CreatedAt) 
                .FirstOrDefaultAsync();

            var viewModel = new HomeIndexViewModel
            {
                FeaturedCvs = users,
                LatestProject = latestProject
            };

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
