using Microsoft.AspNetCore.Identity;

namespace LearnIn.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

        public string? ImagePath { get; set; }

        public DateTime DateOfBirth { get; set; }

        public ICollection<Course> Courses { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }

    }
}
