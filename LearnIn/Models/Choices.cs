namespace LearnIn.Models
{
    public class Choices
    {
        public int Id { get; set; }

        public string ChoiceText { get; set; }

        public bool IsCorrect { get; set; }

        public int QuestionId { get; set; }

        public Question Question { get; set; }
    }
}
