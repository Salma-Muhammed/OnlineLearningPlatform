using LearnIn.Data;
using LearnIn.Models;
using LearnIn.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LearnIn.Controllers
{
    [Authorize(Roles = "Instructor")]
    public class CoursesController : Controller
    {
        private readonly LearnInContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CoursesController(LearnInContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Courses/AllCourses
        [AllowAnonymous]
        public async Task<IActionResult> AllCourses()
        {
            var courses = await _context.Courses
                .Include(c => c.ApplicationUser)
                .ToListAsync();

            ViewBag.Courses = courses;
            return View(courses);
        }

        // GET: Courses/MyCourses
        public async Task<IActionResult> MyCourses()
        {
            var userId = _userManager.GetUserId(User); 
            var courses = await _context.Courses
                .Where(c => c.UserId == userId) // Only courses created by this instructor
                .Include(c => c.ApplicationUser)
                .ToListAsync();

            return View(courses);
        }


        public async Task<IActionResult> CreateCourse()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.InstructorId = user?.Id; // Get the instructor's ID for later use
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCourse(Course model)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var course = new Course
                {
                    Title = model.Title,
                    Description = model.Description,
                    Duration = model.Duration,
                    Category = model.Category,
                    UserId = userId
                };


                _context.Courses.Add(course);
                await _context.SaveChangesAsync();

                return Redirect("/Home/Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while saving the course. Please try again.");
            }

            return View(model);
        }
    }
}
