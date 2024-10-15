using LearnIn.Data;
using LearnIn.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LearnIn.ViewModels;
using System.Security.Claims;

namespace LearnIn.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly LearnInContext _context;

        public UserProfileController(UserManager<ApplicationUser> userManager, LearnInContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<IActionResult> UserProfile()
        {
            var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var roles = await _userManager.GetRolesAsync(user);

            if (user == null)
            {
                return Redirect("/Account/Login");
            }

            var userInfo = new UserRoleViewModel
            {
                ApplicationUser = user,
                Roles = roles // Passing the roles to the view
            };
            return View(userInfo);
        }


        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Redirect("/Account/Login");
            }

            // Get the user's profile from the database
            var userInfo = await _context.Users.FindAsync(user.Id);

            if (userInfo == null)
            {
                return NotFound();
            }

            return View(userInfo); // Return user info to edit view
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(ApplicationUser model, IFormFile ImageFile)
        {
            // Get the current logged-in user
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Redirect("/Account/Login");
            }

            // Find the current user in the database
            var userInfo = await _context.Users.FindAsync(user.Id);

            if (userInfo == null)
            {
                return NotFound();
            }

            // Check if the form model state is valid
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else {    
                // Update the user's profile information with the new data
                userInfo.UserName = model.UserName;
                userInfo.Email = model.Email;
                userInfo.PhoneNumber = model.PhoneNumber;

                // Handle profile picture upload if provided
                //if (ImageFile != null && ImageFile.Length > 0)
                //{
                //    var fileName = Path.GetFileName(ImageFile.FileName);
                //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                //    using (var stream = new FileStream(filePath, FileMode.Create))
                //    {
                //        await ImageFile.CopyToAsync(stream);
                //    }

                //    // Set the new image path
                //    userInfo.Image = "/images/" + fileName;
                //}

                // Save changes to the database
                _context.Users.Update(userInfo);
                await _context.SaveChangesAsync();

                // Redirect back to the profile page to reflect the updated information
                return RedirectToAction("UserProfile"); // Ensure it redirects to the UserProfile action
            }
        }

    }
}
