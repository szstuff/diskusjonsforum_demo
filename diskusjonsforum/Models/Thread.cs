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
        public int UserId { get; set; }
        public virtual User? User { get; set; }  = default!; //User skal egt IKKE være nullable (?), men får invalid ModelState hvis den ikke er det. Løsning: https://stackoverflow.com/questions/70966537/modelstate-isvalid-includes-a-navigation-property-always-false-only-net-6-0
        public List<Comment>? ThreadComments { get; set; }
    }
}
