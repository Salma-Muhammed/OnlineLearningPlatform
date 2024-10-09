using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace LearnIn.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Image { get; set; }
        public int Age { get; set; }
        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
        public virtual ICollection<Enroll> Enrolls { get; set; } = new List<Enroll>();
        public virtual ICollection<Teach> Teaches { get; set; } = new List<Teach>();
        public virtual ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
        public virtual ICollection<StudentTakesExam> StudentTakesExams { get; set; } = new List<StudentTakesExam>();

        [Required]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "Username must be between 4 and 100 characters.")]
        public override string UserName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public override string Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number.")]
        public override string PhoneNumber { get; set; }
    }
}