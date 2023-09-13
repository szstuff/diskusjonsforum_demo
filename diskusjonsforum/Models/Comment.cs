using System;
namespace diskusjonsforum.Models
{
	public class Comment
	{
		public int CommentID { get; set; }
		public String Body { get; set; }
		public int parentID { get; set; } //Parent ID, either post or comment
		public int UserID { get; set; } //FK: Creator UserID 
	}
}
