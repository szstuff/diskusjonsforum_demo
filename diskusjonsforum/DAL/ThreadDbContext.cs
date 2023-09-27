using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace diskusjonsforum.Models;

public class ThreadDbContext : IdentityDbContext
//public class ThreadDbContext : DbContext  // Changed to IdentityDbContext as described in canvas -> Demo:Authentication and authorisation

{
	public ThreadDbContext(DbContextOptions<ThreadDbContext> options) : base(options)
	{
		//Database.EnsureDeleted(); //Deletes database on each run to ensure it's "refreshed" every time 
		Database.EnsureCreated();
	}

    public DbSet<Thread> Threads { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<User> Users { get; set; }
}
