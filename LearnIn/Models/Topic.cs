namespace LearnIn.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; } // Content can be a URL to the video/file or a path
        public ContentType ContentType { get; set; } // Enum to indicate if it's a video or file

        public ICollection<CourseTopic> CourseTopics { get; set; }
    }
    public enum ContentType
    {
        Video,
        File
    }
}
