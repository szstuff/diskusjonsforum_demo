//using Microsoft.EntityFrameworkCore;
//namespace diskusjonsforum.Models
//{
//	public class DiscussionDbContext : DbContext
//	{
//		public DiscussionDbContext(DbContextOptions<DiscussionDbContext> options): base(options)
//		{
//			Database.EnsureCreated();
//		}

//        public DbSet<DiscussionThread> Threads { get; set; }
//		public DbSet<Post> Posts { get; set; }
//	}
//}

//// Må skrives om til å passe til User.
//// Alle entities skal ha egen dbcontext