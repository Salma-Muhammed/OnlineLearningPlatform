using System.ComponentModel.DataAnnotations;

namespace LearnIn.Models
{
    public class RegisterViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string? PhoneNumber { get; set; }
        public int Age { get; set; }
        [Required]
        [DataType(DataType.Upload)]
        [FileExtensions(Extensions = "jpg,jpeg,png")]
        public IFormFile ImageFile { get; set; }
    }
}
