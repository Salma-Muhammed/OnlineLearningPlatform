using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using LearnIn.Data;
using LearnIn.Models;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LearnIn.Controllers
{
    public class CoursesController : Controller
    {
        private readonly LearnInContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CoursesController(LearnInContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Courses
        public async Task<IActionResult> CoursesList()
        {
            var courses = await _context.Courses.ToListAsync(); // Get all courses for display to users
            return View(courses);
        }

        // GET: Instructor's Courses
        public async Task<IActionResult> MyCourses()
        {
            var user = await _userManager.GetUserAsync(User); // Get the currently logged-in user

            if (user == null)
            {
                return RedirectToAction("Login", "Account"); // Redirect to login if no user is found
            }

            // Retrieve courses taught by the instructor
            var instructorCourses = await _context.Teaches
                .Include(t => t.Course) // Include course data
                .Where(t => t.UserId == user.Id)
                .Select(t => t.Course) // Select the Course objects
                .ToListAsync();

            return View(instructorCourses); // Return the list of courses for the instructor
        }


        // GET: Courses/CreateCourse
        public async Task<IActionResult> CreateCourse()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.InstructorId = user?.Id; // Get the instructor's ID for later use
            return View();
        }

        // POST: Courses/CreateCourse
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCourse(Course course, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                // Get the current instructor's ID as a string
                var instructorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Save the image if uploaded
                if (imageFile != null && imageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(imageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    course.Image = "/images/" + fileName; // Store the path in the model
                }

                // Set the instructor ID as a string
                course.InstructorId = instructorId; // Assign the instructor ID directly

                // Add the course to the database
                _context.Courses.Add(course);
                await _context.SaveChangesAsync();

                return RedirectToAction("MyCourses"); // Redirect to the instructor's courses
            }

            return View(course); // Return the view with the course model if invalid
        }




        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Course course, IFormFile imageFile)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var fileName = Path.GetFileName(imageFile.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        course.Image = "/images/" + fileName; // Update the image path
                    }

                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }
    }
}
