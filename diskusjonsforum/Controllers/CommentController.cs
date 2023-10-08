using Microsoft.AspNetCore.Mvc;
using Diskusjonsforum.Models;
using diskusjonsforum.ViewModels;
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
    public IActionResult Create(int commentId, int threadId)
    {
        Comment parentComment = _threadDbContext.Comments.FirstOrDefault(c=>c.CommentId == commentId);
        Thread thread = _threadDbContext.Threads.FirstOrDefault(t => t.ThreadId == threadId);
        // Retrieve query parameters
        // Create a CommentViewModel and populate it with data
        var viewModel = new CommentCreateViewModel()
        {
            ThreadId = threadId,
            ParentCommentId = commentId,
            ParentComment = parentComment,
            Thread = thread
        };

        // Pass the CommentViewModel to the view
        return View(viewModel);
    }
    [HttpPost("comment/save")]
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
   

}

