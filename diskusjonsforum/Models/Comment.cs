using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite;namespace Diskusjonsforum.Models
{
    public class Comment
	{
		public int CommentId { get; set; }
        [Required]
        public String? CommentBody { get; set; }

        public DateTime CommentCreatedAt { get; set; } = DateTime.Now;
        public int ThreadId { get; set; }

        public virtual Thread? Thread { get; set; } = default!;  //skal egt IKKE være nullable (?), men får invalid ModelState hvis den ikke er det. Løsning: https://stackoverflow.com/questions/70966537/modelstate-isvalid-includes-a-navigation-property-always-false-only-net-6-0

        public int? ParentCommentId { get; set; }
        public virtual Comment? ParentComment { get; set; } = default!; //FK av samme klasse må deklareres slik: https://stackoverflow.com/questions/7585076/code-first-with-an-existing-database
        //ParentComment skal egt IKKE være nullable (?), men får invalid ModelState hvis den ikke er det. Løsning: https://stackoverflow.com/questions/70966537/modelstate-isvalid-includes-a-navigation-property-always-false-only-net-6-0
        
        
        public int UserId { get; set; }
        public virtual User? User { get; set; }  = default!;  //User skal egt IKKE være nullable (?), men får invalid ModelState hvis den ikke er det. Løsning: https://stackoverflow.com/questions/70966537/modelstate-isvalid-includes-a-navigation-property-always-false-only-net-6-0
    }
}

