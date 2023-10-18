using Microsoft.AspNetCore.Mvc;
using Diskusjonsforum.Models;
using diskusjonsforum.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Thread = Diskusjonsforum.Models.Thread;


//using diskusjonsforum.ViewModels; //Kan slettes hvis vi ikke lager ViewModels

namespace diskusjonsforum.Controllers;


public class CommentController : Controller
{
    private readonly ThreadDbContext _threadDbContext;

    public CommentController(ThreadDbContext threadDbContext)
    {
        _threadDbContext = threadDbContext;
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
        Comment parentComment = _threadDbContext.Comments.FirstOrDefault(c => c.CommentId == parentCommentId); //Throws no exception because parentComment should be null when the user replies to the thread 
        Thread thread = _threadDbContext.Threads.FirstOrDefault(t => t.ThreadId == threadId) ?? throw new InvalidOperationException("Requested thread not found. ThreadId: " + threadId);
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
    [HttpPost("comment/save")]
    [Authorize]
    public async Task<IActionResult> Save(Comment comment)
    {
        if (ModelState.IsValid)
        {
            _threadDbContext.Comments.Add(comment);
            await _threadDbContext.SaveChangesAsync();
            return RedirectToAction("Thread", "Thread", new {comment.ThreadId});
        }

        return RedirectToAction("Thread", "Thread", new {comment.ThreadId});
    }
    
    [HttpGet("edit/{{commentId}}/{{threadId}}")]
    [Authorize]
    public IActionResult Edit(int commentId, int threadId)
    {
        Comment commentToEdit = _threadDbContext.Comments.FirstOrDefault(c=>c.CommentId == commentId) ?? throw new InvalidOperationException("Requested comment not found. commentId:" + commentId);
        Comment parentComment =
            _threadDbContext.Comments.FirstOrDefault(c => c.CommentId == commentToEdit.ParentCommentId); //Throws no exception because parentComment should be null when the user replies to the thread 
        Thread thread = _threadDbContext.Threads.FirstOrDefault(t => t.ThreadId == threadId) ?? throw new InvalidOperationException("Requested thread not found. ThreadId: " + threadId);
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
    [HttpPost("comment/saveEdit")]
    [Authorize]
    public async Task<IActionResult> SaveEdit(Comment comment)
    {
        if (ModelState.IsValid)
        {
            _threadDbContext.Comments.Update(comment);
            await _threadDbContext.SaveChangesAsync();
            return RedirectToAction("Thread", "Thread", new {comment.ThreadId});
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

