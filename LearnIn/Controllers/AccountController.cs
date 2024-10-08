using LearnIn.Models;
using LearnIn.ViewModel;
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
            return View();
        }

        //to add new user to the DB
        [HttpPost]
        public async Task<IActionResult> SaveSignUp(SignUpViewModel user)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                return View("SignUp",user);
            }
            ApplicationUser NewUser = new ApplicationUser();
            NewUser.UserName = user.UserName;
            NewUser.PasswordHash = user.Password;
            NewUser.Email = user.Email;
            NewUser.DateOfBirth = user.DateOfBirth;
            // Create the user with the plain password
            var result = await _userManager.CreateAsync(NewUser, user.Password);

            if (result.Succeeded)
            {
                // Optionally, sign in the user immediately after registration
                await _signInManager.SignInAsync(NewUser, isPersistent: false);
                return RedirectToAction("Index","Home");
            }

            // If there are errors, add them to the model state
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            // Return the view with the model to display errors
            return View("SignUp",user);
        }
        //----------------My Account--------------------//
        public IActionResult MyAccount()
        {
            return View();
        }
        //-------------------Log In---------------------//
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
        //-------------------Log Out---------------------//
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("LogIn","Account");
        }
        //-------------------Add Role---------------------//
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AddRole()
        {
            return View();
        }
                [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddRole(string RoleName) //Authorized to Admin
        {
            if (string.IsNullOrEmpty(RoleName))
            {
                ViewBag.ErrorMessage = "Role Name Cannot Be Empty.";
                return View();
            }
            IdentityRole role = new IdentityRole {
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
        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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





        public IActionResult ValidateDateOfBirth(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;

            if (dateOfBirth > today.AddYears(-age))
            {
                age--;
            }

            if (age < 18 || age > 60)
            {
                return Json($"Your age must be between 18 and 60 years.");
            }

            return Json(true); // Validation successful
        }
    }
}
