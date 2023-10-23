﻿
using Microsoft.AspNetCore.Mvc;
using Diskusjonsforum.Models;
using diskusjonsforum.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Thread = Diskusjonsforum.Models.Thread;


//using diskusjonsforum.ViewModels; //Kan slettes hvis vi ikke lager ViewModels

namespace diskusjonsforum.Controllers;


public class CommentController : Controller
{
    private readonly ThreadDbContext _threadDbContext;
    private readonly UserManager<ApplicationUser> _userManager;


    public CommentController(ThreadDbContext threadDbContext, UserManager<ApplicationUser> userManager)
    {
        _threadDbContext = threadDbContext;
        _userManager = userManager;

    }

    public async Task<List<Comment>> GetComments()
    {
        var comments = new List<Comment>();
        return comments;
    }
    
    [HttpGet("create/{{commentId}}/{{threadId}}")]
    [Authorize]
    public IActionResult Create(int parentCommentId, int threadId)
    {

        if (HttpContext.User.Identity!.IsAuthenticated)
        {
            Comment parentComment =
                _threadDbContext.Comments.FirstOrDefault(c =>
                    c.CommentId ==
                    parentCommentId)!; //Throws no exception because parentComment should be null when the user replies to the thread 
            Thread thread = _threadDbContext.Threads.FirstOrDefault(t => t.ThreadId == threadId) ??
                            throw new InvalidOperationException("Requested thread not found. ThreadId: " + threadId);
            // Retrieve query parameters
            // Create a CommentViewModel and populate it with data
            var viewModel = new CommentCreateViewModel()
            {
                ThreadId = threadId,
                ParentCommentId = parentCommentId,
                ParentComment = parentComment,
                Thread = thread
            };

            // Pass the CommentViewModel to the view
            return View(viewModel);
        }
        else
        {
            return View("/Areas/Identity/Pages/Account/Login.cshtml");
        }
    }
    [HttpPost("comment/save")]
    [Authorize]
    public async Task<IActionResult> Save(Comment comment)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user != null)
        {
            comment.UserId = user.Id;
            comment.User = user;
            ModelState.Remove("comment.User"); //Workaround for invalid modelstate. The model isnt really invalid, but it was evaluated BEFORE the controller added User and UserId. Therefore the validty of the "User" key can be removed 
            
            if (ModelState.IsValid)
            {
                _threadDbContext.Comments.Add(comment);
                await _threadDbContext.SaveChangesAsync();
                return RedirectToAction("Thread", "Thread", new { comment.ThreadId });
            }
        } //Må legge til else her for feilmeldigner 

        return RedirectToAction("Thread", "Thread", new {comment.ThreadId});
    }
    
    [HttpGet("edit/{{commentId}}/{{threadId}}")]
    [Authorize]
    public IActionResult Edit(int commentId, int threadId)
    {
        if (HttpContext.User.Identity!.IsAuthenticated)
        {
            Comment commentToEdit = _threadDbContext.Comments.FirstOrDefault(c => c.CommentId == commentId) ??
                                    throw new InvalidOperationException("Requested comment not found. commentId:" +
                                                                        commentId);
            Comment parentComment =
                _threadDbContext.Comments.FirstOrDefault(c =>
                    c.CommentId ==
                    commentToEdit
                        .ParentCommentId); //Throws no exception because parentComment should be null when the user replies to the thread 
            Thread thread = _threadDbContext.Threads.FirstOrDefault(t => t.ThreadId == threadId) ??
                            throw new InvalidOperationException("Requested thread not found. ThreadId: " + threadId);
            // Retrieve query parameters
            // Create a CommentViewModel and populate it with data
            var viewModel = new CommentCreateViewModel()
            {
                ThreadId = threadId,
                ParentCommentId = commentId,
                ParentComment = parentComment,
                CommentToEdit = commentToEdit,
                Thread = thread
            };

            // Pass the CommentViewModel to the view
            return View(viewModel);
        }
        else
        {
            return View("/Areas/Identity/Pages/Account/Login.cshtml");
        }
    }
    [HttpPost("comment/saveEdit")]
    [Authorize]
    public async Task<IActionResult> SaveEdit(Comment comment)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user != null)
        {
            comment.User = user;
            comment.UserId = user.Id;
            ModelState.Remove("comment.User"); //Workaround for invalid modelstate. The model isnt really invalid, but it was evaluated BEFORE the controller added User and UserId. Therefore the validty of the "User" key can be removed 

            if (ModelState.IsValid)
            {
                _threadDbContext.Comments.Update(comment);
                await _threadDbContext.SaveChangesAsync();
                return RedirectToAction("Thread", "Thread", new {comment.ThreadId});
            }
        }

        return RedirectToAction("Thread", "Thread", new {comment.ThreadId});
    }

    public async Task<IActionResult> DeleteComment(int commentId)
    {
        Comment comment = _threadDbContext.Comments.FirstOrDefault(c => c.CommentId == commentId) ?? throw new InvalidOperationException("Requested comment not found. CommentId: " + commentId);
        List<Comment> childcomments = AddChildren(comment);

        foreach (var child in childcomments)
        {
            _threadDbContext.Comments.Remove(child);
            await _threadDbContext.SaveChangesAsync();
        }

        _threadDbContext.Comments.Remove(comment);
        await _threadDbContext.SaveChangesAsync();

        return RedirectToAction("Thread", "Thread", new {comment.ThreadId});

        List<Comment> AddChildren(Comment parentComment)
        {
            List<Comment> newChildren = _threadDbContext.Comments.Where(c => c.ParentCommentId == parentComment.CommentId).ToList();
            List<Comment> newerChildren = new List<Comment>();
            foreach (Comment child in newChildren)
            {
                newerChildren.AddRange(AddChildren(child));
            }
            newChildren.AddRange(newerChildren);
            return newChildren;
        }
    }

}

