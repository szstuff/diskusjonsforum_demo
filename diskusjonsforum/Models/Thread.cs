using System.ComponentModel.DataAnnotations.Schema;
using SQLite;
using SQLitePCL;

namespace diskusjonsforum.Models
{
    public class Thread
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; } = null!; //null! "promises" the compiler that the value wont be null. Use Regex!
        public string Category { get; set; } = null!; //null! "promises" the compiler that the value wont be null. Use Regex!
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        [ForeignKey("User")]
        public int CreatedBy { get; set; }
        public ICollection<Comment>? Posts { get; set; }
    }
}
