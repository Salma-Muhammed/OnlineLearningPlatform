using System.ComponentModel.DataAnnotations.Schema;

namespace LearnIn.Models
{
    public class Enroll
    {
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        [ForeignKey("CourseId")]
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public int Progress { get; set; }
        public string Feedback { get; set; }
    }
}
