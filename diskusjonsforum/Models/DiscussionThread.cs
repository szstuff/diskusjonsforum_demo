namespace diskusjonsforum.Models
{
    public class DiscussionThread
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Post> Posts { get; set; }   
    }
}
