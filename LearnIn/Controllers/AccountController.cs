using LearnIn.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LearnIn.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult SignUP()
        {
            //user name, email, phone, password
            ApplicationUser ApplicationUser = new ApplicationUser();
            return View(ApplicationUser);
        }
        //to add new user to the DB
        [HttpPost]
        public async Task<IActionResult> SignUp(ApplicationUser user, string password)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            // Create the user with the plain password
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // Optionally, sign in the user immediately after registration
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Redirect("/Account/LogIn");
            }

            // If there are errors, add them to the model state
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            // Return the view with the model to display errors
            return View(user);
        }


        public IActionResult LogIn()
        {
            var user = new ApplicationUser();
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(string userName, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(userName, password, false, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Status = 1;
                ViewBag.message = "User name or password is incorrect";
            }

            return View();
        }


    }
}
