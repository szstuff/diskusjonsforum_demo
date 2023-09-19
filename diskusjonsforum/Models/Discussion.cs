
namespace diskusjonsforum.Models
{
    public class Discussion
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!; //null! "promises" the compiler that the value wont be null. Use Regex!
        public string Category { get; set; } = null!; //null! "promises" the compiler that the value wont be null. Use Regex!
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public User CreatedBy { get; set; } = null!; //null! "promises" the compiler that the value wont be null. Use Regex!
        public ICollection<Comment>? Posts { get; set; }
    }
}
