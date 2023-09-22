using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite;namespace diskusjonsforum.Models
{
    public class Comment
	{
        [PrimaryKey, AutoIncrement]
		public int CommentId { get; set; }
        [Required]
        public String? Body { get; set; }
        public DateTime CreatedAt { get; set; }
        [ForeignKey("Thread")]
        public Thread Parent { get; set; } = null!; //null! "promises" the compiler that the value wont be null. Use Regex!

        public int? ParentCommentId { get; set; } 

        //Parent thread
        [ForeignKey("User")]
        public User CreatedBy { get; set; } = null!; //null! "promises" the compiler that the value wont be null. Use Regex!
    }
}

