using Microsoft.EntityFrameworkCore;

namespace Diskusjonsforum.Models;

public static class DBInit
{
    public static void Seed(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        ThreadDbContext context = serviceScope.ServiceProvider.GetRequiredService<ThreadDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        if (!context.Users.Any())
        {
            var users = new List<User>
            {
                new User {Administrator = true, PasswordHash = "Stilian", Email = "hei@gmail.com", Name = "Stilian"},
                new User {Administrator = true, PasswordHash = "Saloni", Email = "hei2@gmail.com", Name = "Saloni"},

            };
            context.AddRange(users);
            context.SaveChanges();
        }
        
        if (!context.Threads.Any())
        {
            var threads = new List<Thread>
            {
                new Thread {ThreadTitle = "Hei1", 
                    ThreadBody = "heiiiiii", 
                    ThreadCategory = "Kategori1", 
                    ThreadCreatedAt = Convert.ToDateTime(DateTime.Today.ToString("dd-MM-yyyy")),
                    UserId = 1}
            };
            context.AddRange(threads);
            context.SaveChanges();
        }
        
        if (!context.Comments.Any())
        {
            var comments = new List<Comment>
            {
                new Comment {
                    CommentBody = "Hahahahahahaahhah 😂😂😂😂",
                    CommentCreatedAt = Convert.ToDateTime(DateTime.Today.ToString("dd-MM-yyyy")),
                    ParentCommentId = null,
                    ThreadId = 1,
                    UserId = 1 },
                new Comment {
                    CommentBody = "Hahahahah😂😂",
                    CommentCreatedAt = Convert.ToDateTime(DateTime.Today.ToString("dd-MM-yyyy")),
                    ParentCommentId = 1,
                    ThreadId = 1,
                    UserId = 2 },
                new Comment {
                    CommentBody = ":(",
                    CommentCreatedAt = Convert.ToDateTime(DateTime.Today.ToString("dd-MM-yyyy")),
                    ParentCommentId = 2,
                    ThreadId = 1,
                    UserId = 1 },
                new Comment {
                    CommentBody = "døøøøøde",
                    CommentCreatedAt = Convert.ToDateTime(DateTime.Today.ToString("dd-MM-yyyy")),
                    ParentCommentId = null,
                    ThreadId = 1,
                    UserId = 2 },
                
            };
            context.AddRange(comments);
            context.SaveChanges();
        }
        
    }
    
}