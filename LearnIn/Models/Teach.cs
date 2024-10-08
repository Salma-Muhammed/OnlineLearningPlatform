using System.ComponentModel.DataAnnotations.Schema;

namespace LearnIn.Models
{
    public class Teach
    {
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        [ForeignKey("CourseId")]
        public int CourseId { get; set; }
    }
}
