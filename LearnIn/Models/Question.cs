namespace LearnIn.Models
{
    public class Question
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public ICollection<Choices> Choices { get; set; }

        public int QuizId { get; set; }

        public Quiz Quiz { get; set; }
    }
}
