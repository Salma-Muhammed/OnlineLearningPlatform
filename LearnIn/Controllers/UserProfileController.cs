using LearnIn.Data;
using LearnIn.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var userInfo = await _context.Users.FindAsync(user.Id);

            return View(userInfo);
        }
    }
}
