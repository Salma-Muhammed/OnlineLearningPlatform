namespace LearnIn.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public string? Image { get; set; }
        public string Category { get; set; }
        public ICollection<Enroll> Enrolls { get; set; }
        public ICollection<Teach> Teaches  { get; set; }
        public ICollection<ApplicationUser> ApplicationUsers { get; set; }
        public ICollection<Topic> Topics { get; set; }
    }
}
