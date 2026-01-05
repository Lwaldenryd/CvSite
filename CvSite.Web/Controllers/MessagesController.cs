using CvSite.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CvSite.Web.Data.Entities;

namespace CvSite.Web.Controllers
{
    [Authorize]   
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MessagesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var userId = _userManager.GetUserId(User);

                var Messages = await _context.Messages
                    .Where(m => m.ReceiverId == userId)
                    .OrderByDescending(m => m.SentAt)
                    .ToListAsync();

                return View(Messages);
            }
            catch (Exception)
            {
                return View("Error", "Kunde inte hämta meddelanden.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            try
            {
                var message = await _context.Messages.FindAsync(id);

                if (message == null)
                    return NotFound();

                message.IsRead = true;
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return View("Error", "Ett fel inträffade vid uppdateringen av meddelandet.");
            }
        }
    }
}
