using System.ComponentModel.DataAnnotations.Schema;

namespace LearnIn.Models
{
    public class CourseCategory
    {
        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
        [ForeignKey("CourseId")]
        public int CourseId { get; set; }

    }
}
