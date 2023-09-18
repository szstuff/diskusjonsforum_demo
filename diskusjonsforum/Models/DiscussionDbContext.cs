using System;
using Microsoft.EntityFrameworkCore;

namespace diskusjonsforum.Models;

public class DiscussionDbContext : DbContext
{
	public DiscussionDbContext(DbContextOptions<DiscussionDbContext> options) : base(options)
	{
		//Database.EnsureCreated();
	}

    public DbSet<Discussion> Discussions { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<User> Users { get; set; }
}
