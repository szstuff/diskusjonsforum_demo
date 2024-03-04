using Diskusjonsforum.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Thread = Diskusjonsforum.Models.Thread;

namespace diskusjonsforum.DAL;

public class ThreadDbContext : DbContext
{
	public ThreadDbContext(DbContextOptions<ThreadDbContext> options) : base(options)
	{
	}

    public DbSet<Thread> Threads { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
	    base.OnModelCreating(builder);
	    

	    builder.Entity<Thread>().HasData(
		    new Thread {CategoryName = "General", ThreadBody = "This is a live demo of the diskusjonsforum project developed as part of the ITPE3200 course. Some features have been removed for demo purposes (mainly identity).", ThreadTitle = "Welcome", UserCookie = "stilian", ThreadId = 1}, new Thread {CategoryName = "Help", ThreadBody = "This thread shouldn't be visible to you.", ThreadTitle = "Uh-oh", UserCookie = "obama", ThreadId = 2});
	    builder.Entity<Comment>().HasData(
		    new Comment
		    {
			    CommentBody =
				    "Any comments and threads you post here are only visible to you and only accessible until your session expires.",
			    ThreadId = 1, UserCookie = "stilian", CommentId = 1
		    }, new Comment
		    {
			    CommentBody =
				    "Comments can be replied to. Only you can edit or delete your own comments and threads.",
			    ThreadId = 1, UserCookie = "stilian", CommentId = 2, ParentCommentId = 1
		    }, new Comment
		    {
			    CommentBody =
				    "This comment shouldn't be visible to you :(",
			    ThreadId = 1, UserCookie = "obama", CommentId = 3
		    }, new Comment
		    {
			    CommentBody =
				    "This comment shouldn't be visible to you either.",
			    ThreadId = 2, UserCookie = "obama", CommentId = 4
		    });

    }
    // Method to reset the database
    public void ResetDatabase()
    {
	    Database.EnsureDeleted();
	    Database.EnsureCreated();
    }
}


