using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite;namespace Diskusjonsforum.Models
{
    public class Comment
	{
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int CommentId { get; set; }
        [Required]
        public String? CommentBody { get; set; }
        public DateTime CommentCreatedAt { get; set; }
        //[ForeignKey("Thread")]
        public virtual Thread Thread { get; set; } = default!;

        public int? ParentCommentId { get; set; } 

        //Author ID, navigation property
        //[ForeignKey("User")]
        public virtual User User { get; set; } = default!;
    }
}

