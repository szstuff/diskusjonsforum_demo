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
        public DateTime ThreadCreatedAt { get; set; }  = DateTime.Now;
        public DateTime ThreadLastEditedAt { get; set; } = DateTime.Now;

        [ForeignKey("ApplicationUser")]
        public string? UserId { get; set; }
        public virtual ApplicationUser User { get; set; }  = default!; //ApplicationUser skal egt IKKE være nullable (?), men får invalid ModelState hvis den ikke er det. Løsning: https://stackoverflow.com/questions/70966537/modelstate-isvalid-includes-a-navigation-property-always-false-only-net-6-0
        public List<Comment>? ThreadComments { get; set; }
    }
}
