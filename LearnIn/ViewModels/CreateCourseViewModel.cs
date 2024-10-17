using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
namespace LearnIn.ViewModels
{
    public class CreateCourseViewModel
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public IFormFile Image { get; set; } // Change from string to IFormFile

        [Required]
        public string Category { get; set; }

        public List<TopicViewModel> Topics { get; set; } = new List<TopicViewModel>();
    }

    
}
