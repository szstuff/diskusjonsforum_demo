using System;
namespace diskusjonsforum.Models
{
	public class Post
	{
		public int PostID { get; set; }
        public String Title { get; set; }
        //public String Category { get; set; } //Like a subreddit
        public String Body { get; set; }
        public int UserID { get; set; } // Foreign key: creator UserID
        

    }
}

