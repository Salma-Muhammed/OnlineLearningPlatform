namespace LearnIn.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public string Image { get; set; }
        public int ExamId { get; set; }
        public string ExamName { get; set; }
        public DateTime DateTime { get; set; }
        public ICollection<Enroll> Enrolls { get; set; }
        public ICollection<Teach> Teaches  { get; set; }
        public ICollection<CourseCategory> CourseCategories { get; set; }
        public ICollection<ApplicationUser> ApplicationUsers { get; set; }
        public ICollection<Question> Questions { get; set; }
        public ICollection<StudentTakesExam> StudentTakesExams { get; set; }
        public ICollection<CourseTopic> CourseTopics { get; set; }
    }
}
