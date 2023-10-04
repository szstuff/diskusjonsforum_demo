using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite;
using SQLitePCL;

namespace Diskusjonsforum.Models
{
    public class Thread
    {
        //[PrimaryKey, AutoIncrement]
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ThreadId { get; set; }
        public string? ThreadTitle { get; set; } 
        public string? ThreadCategory { get; set; } 
        public string? ThreadBody { get; set; }
        public DateTime ThreadCreatedAt { get; set; }
        //[ForeignKey("User")]
        public virtual User User { get; set; }  = default!;
        [ForeignKey(UserId)]
        public int UserId { get; set; }
        public List<Comment>? ThreadComments { get; set; }
    }
}
