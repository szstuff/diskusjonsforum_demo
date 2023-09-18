﻿using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace diskusjonsforum.Models
{
    public class Comment
	{
		public int CommentID { get; set; }
        [Required]
        public String Title { get; set; } = null!; //null! "promises" the compiler that the value wont be null. Use Regex!
        //public String Category { get; set; } //Like a subreddit
        public String? Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public Discussion Thread { get; set; } = null!; //null! "promises" the compiler that the value wont be null. Use Regex!
                                                              //Parent thread
        public User CreatedBy { get; set; } = null!; //null! "promises" the compiler that the value wont be null. Use Regex!
    }
}

