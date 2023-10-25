using System.ComponentModel;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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

        // Set and create up Admin role
        var roleStore = new RoleStore<IdentityRole>(context);
        roleStore.CreateAsync(new IdentityRole("Admin"));

        // Set up and create users
        var user = new ApplicationUser //Creates a "deleted" user that can be used as a generic user for deleted content. 
        {
            Email = "xxxx@xxxx.com",
            NormalizedEmail = "XXXX@XXXX.COM",
            UserName = "deleted",
            NormalizedUserName = "DELETED",
            EmailConfirmed = false,
            SecurityStamp = Guid.NewGuid().ToString("D"),
            LockoutEnabled = true,
            LockoutEnd = DateTime.Parse("01-01-2099") //Lockout until 2099 as an attempt to prevent unauthorised access 
        };


        if (!context.Users.Any(u => u.UserName == user.UserName))
        {
            var password = new PasswordHasher<ApplicationUser>();
            var hashed = password.HashPassword(user,".tUAG(n&dZ(U1\\S'O#Xkgkp6:G4kL3?");
            user.PasswordHash = hashed;

            var userStore = new UserStore<ApplicationUser>(context);
            var result = userStore.CreateAsync(user);

        }

        
        //Set up categories
        if (!context.Categories.Any())
        {
            var categories = new List<Category>
            {
                new Category
                {
                    CategoryName = "General",
                    CategoryDescription = "For discussions that do not fit in any other category",
                    RestrictedToAdmins = false
                },
                new Category
                {
                    CategoryName = "Help", CategoryDescription = "Post your help requests here!",
                    RestrictedToAdmins = false
                },
                new Category
                {
                    CategoryName = "Moderation", CategoryDescription = "For anything related to admins and moderation",
                    RestrictedToAdmins = true
                },
            };
            context.AddRange(categories);
            context.SaveChanges();
        }
        
        
        /*if (!context.Threads.Any())
        {
            var threads = new List<Thread>
            {
                new Thread {ThreadTitle = "Hei1", 
                    ThreadBody = "Heiiii jeg heter stilian wææææ vi lagde en nettside joooooo 🥹 bill gates is shivering. Denne tråden er redigert ", 
                    CategoryName = "General", 
                    ThreadCreatedAt = Convert.ToDateTime("09/10/2023 11:01:15"),
                    ThreadLastEditedAt = Convert.ToDateTime("09/10/2023 11:03:03"),
                    UserId = "1"},
                new Thread {ThreadTitle = "Hjelp", 
                    ThreadBody = "jeg trenger hjelp med å lage ditt og datt.", 
                    CategoryName = "General", 
                    ThreadCreatedAt = Convert.ToDateTime("09/10/2023 11:01:15"),
                    ThreadLastEditedAt = Convert.ToDateTime("09/10/2023 11:01:15"),
                    UserId = "1"},
                new Thread {ThreadTitle = "stilian er min bestie", 
                    ThreadBody = "stilian er best han er min bestie bebebebe", 
                    CategoryName = "General", 
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
            context.AddRange(comments3);*/
            context.SaveChanges();
        }
        
    }
    
