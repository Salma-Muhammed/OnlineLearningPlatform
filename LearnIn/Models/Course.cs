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
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }

        public ICollection<Enroll> Enrolls { get; set; } = new List<Enroll>();    
        public ICollection<Topic> Topics { get; set; } = new List<Topic>();

    }
}
