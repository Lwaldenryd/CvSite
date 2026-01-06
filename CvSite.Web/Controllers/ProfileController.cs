using CvSite.Web.Data.Entities;
using CvSite.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CvSite.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);

            var model = new EditProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                IsPrivate = user.IsPrivate
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);

            if (model.ProfileImage != null && model.ProfileImage.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await model.ProfileImage.CopyToAsync(memoryStream);
                user.ProfilePicture = memoryStream.ToArray();
            } 
            
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Address = model.Address;
            user.IsPrivate = model.IsPrivate;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
