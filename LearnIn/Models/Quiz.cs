namespace LearnIn.Models
{
    public class Quiz
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Duration { get; set; }

        public bool IsCompleted { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public ICollection<Question> Questions { get; set; }    
    }
}
