using System.ComponentModel.DataAnnotations.Schema;

namespace LearnIn.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public string? Image { get; set; }
        public string Category { get; set; }
        [ForeignKey("InstructorId")]
        public int InstructorId { get; set; }
        public ICollection<Enroll> Enrolls { get; set; } = new List<Enroll>();    
        public ICollection<Teach> Teaches { get; set; } = new List<Teach>();
        public ICollection<ApplicationUser> Instructor { get; set; }
        public ICollection<Topic> Topics { get; set; } = new List<Topic>();
    }
}
