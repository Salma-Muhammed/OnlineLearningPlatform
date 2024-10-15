using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnIn.Models
{
    public class ContactUs
    {
        [Key]
        public int Id { get; set; } // Primary key for each message

        [Required(ErrorMessage = "Name is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Message is required.")]
        public string Message { get; set; }

        // Foreign key to ApplicationUser
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }

        // Reference to the ApplicationUser who sent the message
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
