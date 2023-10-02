using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite;
using SQLitePCL;

namespace diskusjonsforum.Models
{
    public class Thread
    {
        //[PrimaryKey, AutoIncrement]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Title { get; set; } //null! "promises" the compiler that the value wont be null. Use Regex!
        public string? Category { get; set; } //null! "promises" the compiler that the value wont be null. Use Regex!
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        [ForeignKey("User")]
        public int CreatedBy { get; set; }
        public List<Comment>? Comments { get; set; }
    }
}
