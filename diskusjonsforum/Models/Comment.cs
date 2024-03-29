﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace Diskusjonsforum.Models
{
    public class Comment
	{
		public int CommentId { get; set; }
        [Required]
        public String CommentBody { get; set; }

        public DateTime CommentCreatedAt { get; set; } = DateTime.Now;
        public DateTime CommentLastEditedAt { get; set; } = DateTime.Now; //Initialises as DateTime.Now since the last edit was at the time of creation
        [ForeignKey("Thread")]
        public int ThreadId { get; set; }

        public virtual Thread? Thread { get; set; } = default!;  //Nullable as Thread is not always set correctly upon creation of comment. Instead, we set the Thread upon creation as this proved to be more reliable 
        [ForeignKey("Comment")]
        public int? ParentCommentId { get; set; } //Nullable: ParentcCommentId null means that the comment is a direct reply to the thread 
        public virtual Comment? ParentComment { get; set; } = default!; //Nullable for same reason as Thread 
        
        /*
        [ForeignKey("User")]
        public string? UserId { get; set; }
        public virtual User User { get; set; }  = default!;
        */
        public string? UserCookie { get; set; }

    }
}

