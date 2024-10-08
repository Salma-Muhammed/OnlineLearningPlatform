using System.ComponentModel.DataAnnotations.Schema;

namespace LearnIn.Models
{
    public class StudentAnswer
    {
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        [ForeignKey("QuestionId")]
        public int QuestionId { get; set; }
        public string StudentANS { get; set; }
        public int Value { get; set; }
    }
}
