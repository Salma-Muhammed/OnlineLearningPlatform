using LearnIn.Data;
using LearnIn.Models;
using LearnIn.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
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
            return View();
        }

        // GET: Courses/MyCourses
        public async Task<IActionResult> MyCourses()
        {
            var userId = _userManager.GetUserId(User);
            var courses = await _context.Courses
                .Where(c => c.UserId == userId)
                .Include(c => c.Topics)
                .ToListAsync();

            ViewBag.MyCourses = courses;
            return View();
        }

        [HttpGet]
        public IActionResult CreateCourse()
        {
            var model = new CreateCourseViewModel(); // Initialize your model
            ViewBag.Users = _userManager.Users.ToList(); // Assuming you want to pass users to the view
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCourse(CreateCourseViewModel model)
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

                    // Handle file upload
                    //if (model.Image != null && model.Image.Length > 0)
                    //{
                    //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", model.Image.FileName);

                    //    using (var stream = new FileStream(filePath, FileMode.Create))
                    //    {
                    //        await model.Image.CopyToAsync(stream);
                    //    }

                    //    course.Image = model.Image.FileName;
                    //}

                    _context.Courses.Add(course);
                    await _context.SaveChangesAsync();

                    // Add Topics and their contents
                    //if (model.Topics != null && model.Topics.Count > 0)
                    //{
                    //    foreach (var topicViewModel in model.Topics)
                    //    {
                    //        var newTopic = new Topic
                    //        {
                    //            Name = topicViewModel.Name,
                    //            CourseId = course.CourseId // Associate with the course
                    //        };

                    //        _context.Topics.Add(newTopic);
                    //        await _context.SaveChangesAsync(); // Save topic to get the Id for TopicContent

                    //        // Now add TopicContents if they exist
                    //        if (topicViewModel.TopicContents != null)
                    //        {
                    //            foreach (var content in topicViewModel.TopicContents)
                    //            {
                    //                var topicContent = new TopicContent
                    //                {
                    //                    Content = content.Content,
                    //                    ContentType = (Models.ContentType)content.ContentType, // Cast or map here
                    //                    TopicId = newTopic.Id // Associate with the topic
                    //                };
                    //                _context.TopicContents.Add(topicContent);
                    //            }
                    //        }
                    //    }
                    //    await _context.SaveChangesAsync(); // Save all topic contents
                    //}

                    return RedirectToAction(nameof(MyCourses));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving the course. Please try again.");
                }
            
            return View(model);
        }






    }
}
