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
                    .Include(m => m.Sender)
                    .Include(m => m.Receiver)
                    .Where(m => m.ReceiverId == userId || m.SenderId == userId)
                    .OrderByDescending(m => m.SentAt)
                    .ToListAsync();

                return View(Messages);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        public async Task<IActionResult> Send(string receiverId)
        {
            if (string.IsNullOrEmpty(receiverId)) return NotFound();

            var receiver = await _userManager.FindByIdAsync(receiverId);
            if (receiver == null) return NotFound();

            ViewBag.ReceiverName = $"{receiver.FirstName} {receiver.LastName}";

            var message = new Message { ReceiverId = receiverId };
            return View(message);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(Message message)
        {
           
            message.SenderId = _userManager.GetUserId(User);
            message.SentAt = DateTime.Now;
            message.IsRead = false;

           
            ModelState.Remove("SenderId");
            ModelState.Remove("Sender");
            ModelState.Remove("Receiver");
            ModelState.Remove("SentAt");

            if (ModelState.IsValid)
            {
                _context.Messages.Add(message);
                await _context.SaveChangesAsync();

                
                return RedirectToAction(nameof(Index));
            }

            
            var receiver = await _userManager.FindByIdAsync(message.ReceiverId);
            ViewBag.ReceiverName = receiver != null ? $"{receiver.FirstName} {receiver.LastName}" : "Mottagare";

            return View(message);
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
                return View("Error");
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var userId = _userManager.GetUserId(User);

            var message = await _context.Messages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (message == null) return NotFound();

           
            if (message.SenderId != userId && message.ReceiverId != userId)
            {
                return Forbid();
            }

            
            if (!message.IsRead && message.ReceiverId == userId)
            {
                message.IsRead = true;
                await _context.SaveChangesAsync();
            }

            return View(message);
        }
    }
}
