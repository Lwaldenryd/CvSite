using CvSite.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CvSite.Data.Entities;
using CvSite.Web.Models;

namespace CvSite.Web.Controllers
{  
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MessagesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        [Authorize]
        public async Task<IActionResult> Index()
        {
            try
            {
                var userId = _userManager.GetUserId(User);

                var Messages = await _context.Messages
                    .Include(m => m.Sender!)
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

        [AllowAnonymous]
        public async Task<IActionResult> Send(string receiverId)
        {
            if (string.IsNullOrEmpty(receiverId)) return NotFound();

            var receiver = await _userManager.FindByIdAsync(receiverId);
            if (receiver == null) return NotFound();

            ViewBag.ReceiverName = $"{receiver.FirstName} {receiver.LastName}";

            return View(new SendMessageViewModel
            {
                ReceiverId = receiverId 
            });
        }


        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(SendMessageViewModel model)
        {
            var isAuthenticated = User.Identity.IsAuthenticated;

            if (!isAuthenticated && string.IsNullOrWhiteSpace(model.SenderName))
            {
                ModelState.AddModelError("SenderName", "Name is required when sending messages.");
            }

            if (!ModelState.IsValid) 
            {
                var receiver = await _userManager.FindByIdAsync(model.ReceiverId);
                ViewBag.ReceiverName = receiver != null ? $"{receiver.FirstName} {receiver.LastName}" 
                    : "Receiver";

                return View(model);
            }

            var message = new Message
            {
                ReceiverId = model.ReceiverId,
                Subject = model.Subject,
                Content = model.Content,
                SentAt = DateTime.Now,
                IsRead = false
            };

            if (isAuthenticated)
            {
                message.SenderId = _userManager.GetUserId(User);
            }
            else
            {
                message.SenderName = model.SenderName;
            }
            
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Cv", new { id = model.ReceiverId });
        }

        [Authorize]
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

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var userId = _userManager.GetUserId(User);

            var message = await _context.Messages
                .Include(m => m.Sender!)
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
