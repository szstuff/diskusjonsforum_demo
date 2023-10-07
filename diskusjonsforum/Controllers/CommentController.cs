using Microsoft.AspNetCore.Mvc;
using Diskusjonsforum.Models;
using Microsoft.EntityFrameworkCore;
using Thread = Diskusjonsforum.Models.Comment;

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

    [HttpPost]
    public async Task<IActionResult> Create(Comment comment, int threadId)
    {
        if (ModelState.IsValid)
        {
            _threadDbContext.Comments.Add(comment);
            await _threadDbContext.SaveChangesAsync();
            return RedirectToAction("Thread", "Thread", new {threadId});
        }

        return RedirectToAction("Thread", "Thread", new {threadId});
    }
    public IActionResult Comment(int commentId)
    {
        var comment = _threadDbContext.Comments.Include(c => c.Thread).ThenInclude(c => c.User)
            .FirstOrDefault(c => c.CommentId == commentId);

        if (comment == null)
        {
            return NotFound();
        }
        
        return View(comment);

    }
    
    public IActionResult Create(int commentId/*, int threadId*/)
    {
        // Retrieve query parameters
       // ViewData["ThreadId"] = HttpContext.Request.Query["thread"];
        //ViewData["ParentCommentId"] = HttpContext.Request.Query["comment"];
        // Pass the data to the view using a ViewModel or ViewData
        //return View();
        
        var comment = _threadDbContext.Comments.Include(c => c.Thread).ThenInclude(c => c.User)
            .FirstOrDefault(c => c.CommentId == commentId);

        if (comment == null)
        {
            return NotFound();
        }
        
        return View(comment);
    }
}

