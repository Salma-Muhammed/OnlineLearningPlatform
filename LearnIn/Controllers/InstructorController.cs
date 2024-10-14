//using LearnIn.Data;
//using LearnIn.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;

//namespace LearnIn.Controllers
//{
//    [Authorize(Roles = "Instructor")]
//    public class InstructorController : Controller
//    {

//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly LearnInContext _context;

//        public InstructorController(UserManager<ApplicationUser> userManager, LearnInContext context)
//        {
//            _userManager = userManager;
//            _context = context;
//        }

//        public async Task<IActionResult> Index()
//        {
//            var userId = User.Identity.Name;
//            var user = _context.Users.Find(userId);
//            return View("UserProfile",user);
//        }


//        [HttpGet]
//        public IActionResult Edit()
//        {
//            var userId = User.Identity.Name;
//            var user = _context.Users.Find(userId);
//            return View("EditProfile",user);
//        }

//        [HttpPost]
//        public IActionResult Edit(ApplicationUser user)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Users.Update(user);
//                _context.SaveChanges();
//            }
//            return View("EditProfile",user);
//        }

//        [HttpGet]
//        public IActionResult AddCourse()
//        {
//            return View();
//        }

//        [HttpPost]
//        public IActionResult SaveAddCourse(Course course)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Courses.Add(course);
//                _context.SaveChanges();
//                return RedirectToAction("courses");
//            }
//            return View("AddCourse",course);
//        }
//        public IActionResult Courses()
//        {
//            string username = User.Identity.Name;
//            var user = _context.Users.First(u => u.UserName == username);
//            var coursesStudent = _context.Enrolls
//            .Where(c => c.UserId == user.Id) // Filter by instructor
//            .Select(c => new CourseWithStudentCount
//            {
                
//                CourseId = c.CourseId,
//                CourseDuration = c.Course.Duration,
//                CourseName = c.Course.Title,
//                CourseDesc = c.Course.Description,
//                StudentCount = c.Course.StudentTakesExams.Count(),
//            })
//            .ToList();
//            return View(coursesStudent);
//        }


//    }
//}
