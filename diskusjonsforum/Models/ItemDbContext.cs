using Microsoft.EntityFrameworkCore;
namespace diskusjonsforum.Models
{
	public class ItemDbContext : DbContext
	{
		public ItemDbContext(DbContextOptions<ItemDbContext> options): base(options)
		{
			Database.EnsureCreated();
		}

        public DbSet<Post> Posts { get; set; }
	}
}

