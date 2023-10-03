using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite;namespace Diskusjonsforum.Models
{
    public class Comment
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int CommentId { get; set; }
        [Required]
        public String? CommentBody { get; set; }
        public DateTime CommentCreatedAt { get; set; }
        [ForeignKey("Thread")]
        public Thread ThreadId { get; set; }

        public int? ParentCommentId { get; set; } 

        //Parent thread
        [ForeignKey("User")]
        public User UserId { get; set; }
    }
}

