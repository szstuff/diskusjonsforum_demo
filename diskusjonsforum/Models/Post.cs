using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace diskusjonsforum.Models
{
    public class Post
	{
		public int PostID { get; set; }
        [Required]
        public String Title { get; set; }
        //public String Category { get; set; } //Like a subreddit
        public String? Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public DiscussionThread Thread { get; set; } //Parent thread
        public int UserID { get; set; } // Foreign key: creator UserID
    }
}

