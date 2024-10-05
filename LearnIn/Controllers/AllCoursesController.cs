using Microsoft.AspNetCore.Mvc;

namespace LearnIn.Controllers
{
    public class AllCoursesController : Controller
    {
        public IActionResult CoursesList()
        {
            return View();
        }
    }
}
