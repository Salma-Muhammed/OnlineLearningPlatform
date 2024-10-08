using System.ComponentModel.DataAnnotations.Schema;

namespace LearnIn.Models
{
    public class CourseTopic
    {
        [ForeignKey("CourseId")]
        public int CourseId { get; set; }
        public Course Course { get; set; }
        [ForeignKey("TopicId")]
        public int TopicId { get; set; }
        public Topic Topic { get; set; }
    }
}
