using CvSite.Web.Data;
using CvSite.Web.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CvSite.Web.Controllers
{
    [Authorize]
    public class ExperiencesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ExperiencesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var experiences = await _context.Experiences
                .Where(c => c.ApplicationUserId == userId)
                .ToListAsync();

            return View(experiences);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Company,Role,Description,StartDate,EndDate")] Experience experiences)
        {
            var currentUserId = _userManager.GetUserId(User);
            experiences.ApplicationUserId = currentUserId!;

           
            ModelState.Remove("ApplicationUserId");
            ModelState.Remove("ApplicationUser");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(experiences);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("index", "Home"); 
                }
                catch (Exception)
                {
                    
                    ModelState.AddModelError("", "Kunde inte spara erfarenheten på grund av ett databasfel.");
                }
            }
            return View(experiences);
        }
    }
}
