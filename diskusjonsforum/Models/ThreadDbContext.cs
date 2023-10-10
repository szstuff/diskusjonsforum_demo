using System;
using Microsoft.EntityFrameworkCore;

namespace Diskusjonsforum.Models;

public class ThreadDbContext : DbContext
{
	public ThreadDbContext(DbContextOptions<ThreadDbContext> options) : base(options)
	{
		//Database.EnsureDeleted();
		//Database.EnsureCreated();
		
		//Lazy loading (might not work properly) 
		ChangeTracker.LazyLoadingEnabled = true;
		//(Should) enable lazy loading of related entities. This means that entities related to the class you're accessing (foreign keys) are loaded when needed rather than when 
		//instructed to in the controller. E.g. when displaying a comment using comment controller, the associated Thread that's displayed is also loaded automatically 
	}

    public DbSet<Thread> Threads { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<User> Users { get; set; }
    
    
}
