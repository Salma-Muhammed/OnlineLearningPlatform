using System.ComponentModel.DataAnnotations.Schema;

namespace LearnIn.Models
{
    public class Enrollment
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        [ForeignKey("Student")]
        public string StudentId { get; set; }

        public ApplicationUser Student { get; set; }

        public decimal Progress { get; set; }
    }
}
