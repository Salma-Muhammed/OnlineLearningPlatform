using LearnIn.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LearnIn.Controllers
{
    public class ContactUsController : Controller
    {
        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ApplicationUser model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Return the view with validation errors
            }

            // Here you can handle the form submission (e.g., send an email or save to the database)
            // For example, you might call a service to send an email:
            // await _emailService.SendEmailAsync(model.Email, "Contact Us Message", model.Message);

            // Redirect to a thank you page or the same contact page with a success message
            ViewBag.SuccessMessage = "Your message has been sent successfully!";
            return RedirectToAction("Index"); // Optionally redirect to another action if needed
        }
    }
}
