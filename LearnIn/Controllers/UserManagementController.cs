using LearnIn.Data;
using LearnIn.Models;
using LearnIn.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LearnIn.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserManagementController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        LearnInContext _context;
        private readonly ILogger<UserManagementController> _logger;

        public UserManagementController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            LearnInContext context,
            ILogger<UserManagementController> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
            _logger = logger;
        }
        //-------------------DisplayUsers---------//
        public async Task<IActionResult> DisplayUsers()
        {
            // Create a list to store the user role information
            var userRoleList = new List<UserRoleViewModel>();

            // Retrieve all users from the UserManager
            var users = await _userManager.Users.ToListAsync();

            // Loop through each user and fetch their roles
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                // Don't Display Admins
                if (!roles.Contains("Admin"))
                {
                    userRoleList.Add(new UserRoleViewModel
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        Roles = roles.ToList()
                    });
                }
            }

            // Pass the list of users and their roles to the view using ViewBag
            ViewBag.UserRoleList = userRoleList;

            return View();
        }

        //--------------------AddRole-----------------//
        [HttpGet]
        public IActionResult AddRole()
        {
            return View();
        }
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

        //--------------------AssignRole-----------------//
        [HttpGet]
        public async Task<IActionResult> AssignRole()
        {
            ViewBag.Users = (await _userManager.Users.ToListAsync())
                 .Select(u => new SelectListItem { Value = u.Id, Text = u.UserName }).ToList();

            ViewBag.Roles = (await _roleManager.Roles.ToListAsync())
                .Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToList();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, string roleName)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleName))
            {
                ViewBag.ErrorMessage = "User and Role must be selected.";
                return RedirectToAction("AssignRole");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return RedirectToAction("AssignRole", new { error = "User not found." });

            var result = await _userManager.AddToRoleAsync(user, roleName);
            ViewBag.Message = result.Succeeded ? "Role assigned successfully." :
                "Error assigning role: " + string.Join(", ", result.Errors.Select(e => e.Description));

            return RedirectToAction("AssignRole");
        }

        //--------------EditUser----------//
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("DisplayUsers");
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction("DisplayUsers");
            }

            // Pass the user object to the view
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(ApplicationUser model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    // Update user properties
                    user.UserName = model.UserName;
                    user.Age = model.Age;
                    user.PhoneNumber = model.PhoneNumber;
                    user.Email = model.Email;

                    // If you need to change the password, handle it separately
                    if (!string.IsNullOrEmpty(model.PasswordHash))
                    {
                        var passwordHasher = new PasswordHasher<ApplicationUser>();
                        user.PasswordHash = passwordHasher.HashPassword(user, model.PasswordHash);
                    }

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("DisplayUsers");
                    }
                }
            }
            return View(model);
        }
        //------------------------------------------//

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("User ID is not provided.");
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Delete the user
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("DisplayUsers");
            }

            // Handle errors, if any
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View("DisplayUsers");
        }


    }
}
