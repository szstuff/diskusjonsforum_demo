using System;
using Microsoft.EntityFrameworkCore;

namespace diskusjonsforum.Models;

public class ThreadDbContext : DbContext
{
	public ThreadDbContext(DbContextOptions<ThreadDbContext> options) : base(options)
	{
		Database.EnsureCreated();
	}

    public DbSet<Thread> Threads { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<User> Users { get; set; }
}

