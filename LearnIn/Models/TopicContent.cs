﻿using System.ComponentModel.DataAnnotations.Schema;

namespace LearnIn.Models
{
    public class Content
    {
        public int Id { get; set; }
        public string TopicContent { get; set; } // Content can be a URL to the video/file or a path
        public ContentType ContentType { get; set; } // Enum to indicate if it's a video or file
        [ForeignKey("TopicId")]
        public int TopicId { get; set; }
        public virtual Topic Topic { get; set; }
    }
    public enum ContentType
    {
        Video,
        File
    }
}
