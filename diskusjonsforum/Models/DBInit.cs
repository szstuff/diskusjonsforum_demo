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
            var users = new List<ApplicationUser>
            {
                new ApplicationUser {PasswordHash = "Stilian", Email = "hei@gmail.com", UserName = "Stilian"},
                new ApplicationUser {PasswordHash = "Saloni", Email = "hei2@gmail.com", UserName = "Saloni"},
                new ApplicationUser {PasswordHash = "Jovia", Email = "hei3@gmail.com", UserName = "Jovia"},
                new ApplicationUser {PasswordHash = "Jenny", Email = "hei4@gmail.com", UserName = "Jenny"},
                new ApplicationUser {PasswordHash = "Linn", Email = "hei5@gmail.com", UserName = "Linn"},
                new ApplicationUser {PasswordHash = "Huub", Email = "hei6@gmail.com", UserName = "Huub"},

            };
            context.AddRange(users);
            context.SaveChanges();
        }
        
        if (!context.Threads.Any())
        {
            var threads = new List<Thread>
            {
                new Thread {ThreadTitle = "Hei1", 
                    ThreadBody = "Heiiii jeg heter stilian wææææ vi lagde en nettside joooooo 🥹 bill gates is shivering. Denne tråden er redigert ", 
                    ThreadCategory = "Introduksjon", 
                    ThreadCreatedAt = Convert.ToDateTime("09/10/2023 11:01:15"),
                    ThreadLastEditedAt = Convert.ToDateTime("09/10/2023 11:03:03"),
                    UserId = "1"},
                new Thread {ThreadTitle = "Hjelp", 
                    ThreadBody = "jeg trenger hjelp med å lage ditt og datt.", 
                    ThreadCategory = "Hjelp", 
                    ThreadCreatedAt = Convert.ToDateTime("09/10/2023 11:01:15"),
                    ThreadLastEditedAt = Convert.ToDateTime("09/10/2023 11:01:15"),
                    UserId = "1"},
                new Thread {ThreadTitle = "stilian er min bestie", 
                    ThreadBody = "stilian er best han er min bestie bebebebe", 
                    ThreadCategory = "Stilian Appreciation Kategori", 
                    ThreadCreatedAt = Convert.ToDateTime("09/04/2002 08:25:16"),
                    ThreadLastEditedAt = Convert.ToDateTime("10/04/2002 03:25:16"),
                    UserId = "2"}
            };
            context.AddRange(threads);
            context.SaveChanges();
        }
        
        if (!context.Comments.Any())
        {
            var comments3 = new List<Comment>
            {
                new Comment {
                    CommentBody = "stilian er best jo",
                    CommentCreatedAt = Convert.ToDateTime("09/10/2023 11:05:00"),
                    CommentLastEditedAt = Convert.ToDateTime("09/10/2023 11:05:00"),
                    ParentCommentId = null,
                    ThreadId = 3,
                    UserId = "3" },
                new Comment {
                    CommentBody = "OMG SANT 😍",
                    CommentCreatedAt = Convert.ToDateTime("09/10/2023 11:06:00"),
                    CommentLastEditedAt = Convert.ToDateTime("09/10/2023 11:07:15"),
                    ParentCommentId = null,
                    ThreadId = 3,
                    UserId = "5" },
                new Comment {
                    CommentBody = "du er best 🥹",
                    CommentCreatedAt = Convert.ToDateTime("09/10/2023 11:13:10"),
                    CommentLastEditedAt = Convert.ToDateTime("09/10/2023 11:13:10"),
                    ParentCommentId = null,
                    ThreadId = 3,
                    UserId = "1" },
                new Comment {
                    CommentBody = "Enig",
                    CommentCreatedAt = Convert.ToDateTime("09/10/2023 11:15:00"),
                    CommentLastEditedAt = Convert.ToDateTime("09/10/2023 11:25:10"),
                    ParentCommentId = null,
                    ThreadId = 3,
                    UserId = "4" },
                
            };
            
            var comments1 = new List<Comment>
            {
                new Comment {
                    CommentBody = "hei",
                    CommentCreatedAt = Convert.ToDateTime("09/10/2023 11:05:00"),
                    CommentLastEditedAt = Convert.ToDateTime("09/10/2023 11:05:00"),
                    ParentCommentId = null,
                    ThreadId = 1,
                    UserId = "3" },
                new Comment {
                    CommentBody = "Hei. Denne kommentaren er redigert",
                    CommentCreatedAt = Convert.ToDateTime("09/10/2023 11:06:00"),
                    CommentLastEditedAt = Convert.ToDateTime("09/10/2023 11:07:15"),
                    ParentCommentId = null,
                    ThreadId = 1,
                    UserId = "2" },
                new Comment {
                    CommentBody = "hei",
                    CommentCreatedAt = Convert.ToDateTime("09/10/2023 11:13:10"),
                    CommentLastEditedAt = Convert.ToDateTime("09/10/2023 11:13:10"),
                    ParentCommentId = null,
                    ThreadId = 1,
                    UserId = "4" },
                new Comment {
                    CommentBody = "hei. denne kommentarern er redigert",
                    CommentCreatedAt = Convert.ToDateTime("09/10/2023 11:15:00"),
                    CommentLastEditedAt = Convert.ToDateTime("09/10/2023 11:25:10"),
                    ParentCommentId = 2,
                    ThreadId = 1,
                    UserId = "2" },
                
            };
            
            var comments2 = new List<Comment>
            {
                new Comment {
                    CommentBody = "jeg trenger også hjelp",
                    CommentCreatedAt = Convert.ToDateTime("09/10/2023 11:05:00"),
                    CommentLastEditedAt = Convert.ToDateTime("09/10/2023 11:05:00"),
                    ParentCommentId = null,
                    ThreadId = 2,
                    UserId = "5" },
                new Comment {
                    CommentBody = "hjelp meg !!! denne kommentaren er redigert",
                    CommentCreatedAt = Convert.ToDateTime("09/10/2023 11:06:00"),
                    CommentLastEditedAt = Convert.ToDateTime("09/10/2023 11:07:15"),
                    ParentCommentId = 5,
                    ThreadId = 2,
                    UserId = "2" },
                new Comment {
                    CommentBody = "jeg og",
                    CommentCreatedAt = Convert.ToDateTime("09/10/2023 11:13:10"),
                    CommentLastEditedAt = Convert.ToDateTime("09/10/2023 11:13:10"),
                    ParentCommentId = 6,
                    ThreadId = 2,
                    UserId = "4" },
                new Comment {
                    CommentBody = "hjelpes denne kommentaren er redigert",
                    CommentCreatedAt = Convert.ToDateTime("09/10/2023 11:15:00"),
                    CommentLastEditedAt = Convert.ToDateTime("09/10/2023 11:25:10"),
                    ParentCommentId = 7,
                    ThreadId = 2,
                    UserId = "2" },
                new Comment {
                    CommentBody = "jeg trenger også hjelp",
                    CommentCreatedAt = Convert.ToDateTime("09/10/2023 11:05:00"),
                    CommentLastEditedAt = Convert.ToDateTime("09/10/2023 11:05:00"),
                    ParentCommentId = 8,
                    ThreadId = 2,
                    UserId = "1" },
                new Comment {
                    CommentBody = "hjelp meg !!! denne kommentaren er redigert",
                    CommentCreatedAt = Convert.ToDateTime("09/10/2023 11:06:00"),
                    CommentLastEditedAt = Convert.ToDateTime("09/10/2023 11:07:15"),
                    ParentCommentId = 9,
                    ThreadId = 2,
                    UserId = "3" },
                new Comment {
                    CommentBody = "jeg og",
                    CommentCreatedAt = Convert.ToDateTime("09/10/2023 11:13:10"),
                    CommentLastEditedAt = Convert.ToDateTime("09/10/2023 11:13:10"),
                    ParentCommentId = 10,
                    ThreadId = 2,
                    UserId = "1" },
                new Comment {
                    CommentBody = "hjelpes denne kommentaren er redigert",
                    CommentCreatedAt = Convert.ToDateTime("09/10/2023 11:15:00"),
                    CommentLastEditedAt = Convert.ToDateTime("09/10/2023 11:25:10"),
                    ParentCommentId = 11,
                    ThreadId = 2,
                    UserId = "2" },
                new Comment {
                    CommentBody = "jeg trenger også hjelp",
                    CommentCreatedAt = Convert.ToDateTime("09/10/2023 11:05:00"),
                    CommentLastEditedAt = Convert.ToDateTime("09/10/2023 11:05:00"),
                    ParentCommentId = 12,
                    ThreadId = 2,
                    UserId = "4" },
                new Comment {
                    CommentBody = "hjelp meg !!! denne kommentaren er redigert",
                    CommentCreatedAt = Convert.ToDateTime("09/10/2023 11:06:00"),
                    CommentLastEditedAt = Convert.ToDateTime("09/10/2023 11:07:15"),
                    ParentCommentId = 8,
                    ThreadId = 2,
                    UserId = "2" },
                new Comment {
                    CommentBody = "jeg og",
                    CommentCreatedAt = Convert.ToDateTime("09/10/2023 11:13:10"),
                    CommentLastEditedAt = Convert.ToDateTime("09/10/2023 11:13:10"),
                    ParentCommentId = 7,
                    ThreadId = 2,
                    UserId = "1" },
                new Comment {
                    CommentBody = "hjelpes denne kommentaren er redigert",
                    CommentCreatedAt = Convert.ToDateTime("09/10/2023 11:15:00"),
                    CommentLastEditedAt = Convert.ToDateTime("09/10/2023 11:25:10"),
                    ParentCommentId = 6,
                    ThreadId = 2,
                    UserId = "5" },
                
            };
            context.AddRange(comments1);
            context.AddRange(comments2);
            context.AddRange(comments3);
            context.SaveChanges();
        }
        
    }
    
}