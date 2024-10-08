using System.ComponentModel.DataAnnotations.Schema;

namespace LearnIn.Models
{
    public class Answer
    {
        public int AnswerId { get; set; }
        [ForeignKey("QuestionId")]
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public bool IsCorrectAnswer { get; set; }
        public ICollection<Question> Questions { get; set; }
    }
}
