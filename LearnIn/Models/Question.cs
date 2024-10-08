using System.ComponentModel.DataAnnotations.Schema;

namespace LearnIn.Models
{
    public class Question
    {
        public int QuestionId { get; set; }
        public string Text { get; set; }
        [ForeignKey("EXamId")]
        public int ExamId { get; set; }
        public ICollection<StudentAnswer> StudentAnswers { get; set; }

    }
}
