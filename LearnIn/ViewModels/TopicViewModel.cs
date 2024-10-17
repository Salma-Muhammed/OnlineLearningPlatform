using System.ComponentModel.DataAnnotations; // This is required for data annotations

namespace LearnIn.ViewModels
{
    public class TopicViewModel
    {
        [Required]
        public string Name { get; set; }

        public List<TopicContentViewModel> TopicContents { get; set; } = new List<TopicContentViewModel>();
    }
}
