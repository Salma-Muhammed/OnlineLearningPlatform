using System.ComponentModel.DataAnnotations.Schema;

namespace LearnIn.Models
{
    public class StudentTakesExam
    {
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        [ForeignKey("ExamId")]
        public int ExamId { get; set; }
        public string Grade { get; set; }
    }
}
