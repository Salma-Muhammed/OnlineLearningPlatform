using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnIn.Models
{
    public class Teach
    {
        [Key]
        public int Id { get; set; } // Primary key for the Teach table

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; } // Foreign key to the ApplicationUser
        public ApplicationUser Instructor { get; set; } // Navigation property

        [ForeignKey("Course")]
        public int CourseId { get; set; } // Foreign key to the Course
        public Course Course { get; set; } // Navigation property
    }
}
