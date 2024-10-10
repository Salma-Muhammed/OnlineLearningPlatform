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
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        //-------------------Sign Up---------------------//
        public IActionResult SignUp()
        {
            return View(new ApplicationUser());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(ApplicationUser model, string password, string confirmPassword, string Role)
        {
            if (ModelState.IsValid)
            {
                // Handle ImageFile upload
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];
                    if (file.Length > 0)
                    {
                        var filePath = Path.Combine("wwwroot/images", file.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        model.Image = "/images/" + file.FileName;
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
                    if (!string.IsNullOrEmpty(Role))
                    {
                        await _userManager.AddToRoleAsync(model, Role);
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
        //-----------------------------------------------//
        public async Task<IActionResult> RedirectBasedOnRole()
        {
            var user = await _userManager.GetUserAsync(User);

            if (await _userManager.IsInRoleAsync(user, "Instructor"))
            {
                return RedirectToAction("InstructorDashboard", "Instructor");
            }
            else if (await _userManager.IsInRoleAsync(user, "Student"))
            {
                return RedirectToAction("StudentDashboard", "Student");
            }

            return RedirectToAction("Index", "Home");
        }


        //-------------------Log Out---------------------//
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/Home/Index");
        }
        //-------------------Add Role---------------------//
        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AddRole()
        {
            return View();
        }
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddRole(string RoleName) //Authorized to Admin
        {
            if (string.IsNullOrEmpty(RoleName))
            {
                ViewBag.ErrorMessage = "Role Name Cannot Be Empty.";
                return View();
            }
            IdentityRole role = new IdentityRole
            {
                Name = RoleName
            };
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                ViewBag.SuccessMessage = "Role Created Successfully.";
            }
            else
            {
                ViewBag.ErrorMessage = "Error Occured While Creating Role: " + string.Join(", ", result.Errors.Select(e => e.Description));
            }
            return View();
        }
        //-------------------Assign Role---------------------//
        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> AssignRole() //Authorized to Admin
        {
            // Fetch list of users
            var users = _userManager.Users.ToList();

            // Fetch list of roles
            var roles = await _roleManager.Roles.ToListAsync();

            // Pass users and roles to the view using ViewBag
            ViewBag.Users = users.Select(u => new SelectListItem
            {
                Value = u.Id,
                Text = u.UserName
            }).ToList();

            ViewBag.Roles = roles.Select(r => new SelectListItem
            {
                Value = r.Name,
                Text = r.Name
            }).ToList();

            return View();
        }

        // [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, string roleName)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleName))
            {
                ViewBag.ErrorMessage = "User and Role must be selected.";
                return RedirectToAction("AssignRole");
            }

            // Fetch the user by ID
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = "User not found.";
                return RedirectToAction("AssignRole");
            }

            // Add the user to the selected role
            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                ViewBag.SuccessMessage = "Role assigned successfully.";
            }
            else
            {
                ViewBag.ErrorMessage = "Error assigning role: " + string.Join(", ", result.Errors.Select(e => e.Description));
            }

            return RedirectToAction("AssignRole");
        }


    }
}