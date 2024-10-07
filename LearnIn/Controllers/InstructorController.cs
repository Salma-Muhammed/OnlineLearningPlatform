using Microsoft.AspNetCore.Mvc;

namespace LearnIn.Controllers
{
    public class InstructorController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
