using Diskusjonsforum.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Thread = Diskusjonsforum.Models.Thread;

namespace diskusjonsforum.DAL;

public class ThreadDbContext : DbContext
{
	public ThreadDbContext(DbContextOptions<ThreadDbContext> options) : base(options)
	{
		Database.EnsureDeleted();
		Database.EnsureCreated();
	}

    public DbSet<Thread> Threads { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
	    base.OnModelCreating(builder);
	    
	    builder.Entity<Category>().HasKey(c => c.CategoryName);

	    builder.Entity<Thread>();
	    builder.Entity<Thread>()
		    .HasOne(t => t.Category)
		    .WithMany(c => c.ThreadsInCategory)
		    .HasForeignKey(t => t.CategoryName);
	    builder.Entity<Comment>();

    }
}


