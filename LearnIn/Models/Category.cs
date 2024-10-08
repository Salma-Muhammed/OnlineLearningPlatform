namespace LearnIn.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public ICollection<CourseCategory> CourseCategories { get; set;} 
    }
}
