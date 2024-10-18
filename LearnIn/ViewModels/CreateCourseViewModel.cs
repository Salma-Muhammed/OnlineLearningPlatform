using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using LearnIn.Models;

namespace LearnIn.ViewModels
{
    public class CourseViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public string Category { get; set; }

        // List to hold topics
        public List<TopicViewModel> Topics { get; set; } = new List<TopicViewModel>();
    }

    public class TopicViewModel
    {
        public string Name { get; set; } // Only the Name is needed for topic creation
    }


}
