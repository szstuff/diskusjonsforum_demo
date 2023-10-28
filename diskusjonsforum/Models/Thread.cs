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
        public int ThreadId { get; set; } /*sets unique id*/
        public string? ThreadTitle { get; set; } 
        [ForeignKey("Category")] /* sets foreignkey*/
        public string CategoryName { get; set; } /*holds the name of the category to which the thread belongs to.*/
        public Category Category { get; set; } /*navigation property, creates a relationship to the Category entity. accesses the related category for this thread*/
        public string? ThreadBody { get; set; }
        public DateTime ThreadCreatedAt { get; set; }  = DateTime.Now; /*timestamp on when the thread was created*/
        public DateTime ThreadLastEditedAt { get; set; } = DateTime.Now; /*timestamp on when the thread was last edited at*/

        [ForeignKey("ApplicationUser")] /*sets foreignkey. represents a relationship between Thread entity and applicaitonuser entity*/
        public string? UserId { get; set; } /*represents user id*/
        public virtual ApplicationUser User { get; set; }  = default!; //ApplicationUser skal egt IKKE være nullable (?), men får invalid ModelState hvis den ikke er det. Løsning: https://stackoverflow.com/questions/70966537/modelstate-isvalid-includes-a-navigation-property-always-false-only-net-6-0
        public List<Comment>? ThreadComments { get; set; } /*represents collection of comments*/
    }
}
