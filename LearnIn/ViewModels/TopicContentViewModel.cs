using System.ComponentModel.DataAnnotations; // This is required for data annotations

namespace LearnIn.ViewModels
{
    public class TopicContentViewModel
    {
        [Required]
        public string Content { get; set; }

        public ContentType ContentType { get; set; }
    }
    public enum ContentType
    {
        Video,
        File
    }
}
