using LearnIn.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LearnIn.ViewModel
{
    public class SignUpViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password" , ErrorMessage = "The password and confirmation password do not match.")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }


        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date, ErrorMessage = "Please enter a valid date")]
        [Remote(action: "ValidateDateOfBirth", controller: "Account", ErrorMessage = "Your age must be between 12 and 120 years.")]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        public string? PhoneNumber { get; set; }

        [Required]
        public string Role { get; set; }
        public IFormFile? Image { get; set; }
    }
}
