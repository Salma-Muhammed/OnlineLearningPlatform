using Microsoft.AspNetCore.Mvc;

namespace LearnIn.Controllers
{
    public class ContactUsController : Controller
    {
        public IActionResult Contact()
        {
            return View();
        }
    }
}
