using System.ComponentModel.DataAnnotations.Schema;

namespace LearnIn.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey("CourseId")]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
        public ICollection<Content> Contents { get; set; } = new List<Content>();
    }
}
