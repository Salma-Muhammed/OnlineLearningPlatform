using System.ComponentModel.DataAnnotations.Schema;

namespace LearnIn.Models
{

    public class Course
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Duration { get; set; }

        [ForeignKey("Instructor")]
        public string InstructorId { get; set; }

        public ApplicationUser Instructor { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public ICollection<Quiz> Quizzes { get; set; }
    }
}
