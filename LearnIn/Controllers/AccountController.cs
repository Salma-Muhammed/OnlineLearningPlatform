using LearnIn.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace LearnIn.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IWebHostEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _hostingEnvironment = hostingEnvironment;
        }
        //-------------------Sign Up---------------------//
        public IActionResult SignUp()
        {
            return View(new ApplicationUser());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(ApplicationUser model, string password, string confirmPassword, string role)
        {
            if (ModelState.IsValid)
            {
                // Handle ImageFile upload
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];

                    if (file.Length > 0)
                    {
                        // Define the path to save the file
                        var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");

                        // Ensure the "images" directory exists
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        // Generate a unique filename to prevent overwriting existing files
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);

                        // Combine the folder and file name to get the full path
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // Save the file to the server
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // Save the relative path (e.g., "/images/uniqueFileName.jpg") in the database
                        model.Image = "/images/" + uniqueFileName;
                    }
                }

                // Check if passwords match
                if (password != confirmPassword)
                {
                    ModelState.AddModelError(string.Empty, "Password and confirmation password do not match.");
                    return View(model);
                }

                // Create user and assign role
                var result = await _userManager.CreateAsync(model, password);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(role))
                    {
                        await _userManager.AddToRoleAsync(model, role);
                    }

                    // Automatically log in the user
                    await _signInManager.SignInAsync(model, isPersistent: false);

                    // Redirect to home after successful registration and sign-in
                    return RedirectToAction("Index", "Home");
                }

                // Display any errors during the sign-up process
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        //-------------------Log In---------------------//
        public IActionResult LogIn()
        {
            var user = new ApplicationUser();
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(string username, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(username, password, false, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Status = 1;
                ViewBag.message = "Username or password is incorrect";
            }

            return View();
        }
        //----------------------RedirectBasedOnRole-------------------------//
        //public async Task<IActionResult> RedirectBasedOnRole()
        //{
        //    var user = await _userManager.GetUserAsync(User);

        //    if (await _userManager.IsInRoleAsync(user, "Instructor"))
        //    {
        //        return RedirectToAction("InstructorDashboard", "Instructor");
        //    }
        //    else if (await _userManager.IsInRoleAsync(user, "Student"))
        //    {
        //        return RedirectToAction("StudentDashboard", "Student");
        //    }

        //    return RedirectToAction("Index", "Home");
        //}


        //-------------------Log Out---------------------//
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/Home/Index");
        }

    }
}