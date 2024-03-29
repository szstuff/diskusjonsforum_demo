
using diskusjonsforum.DAL;
using Microsoft.AspNetCore.Mvc;
using Diskusjonsforum.Models;
using diskusjonsforum.ViewModels;
using Microsoft.VisualStudio.Shell.Interop;
using Thread = Diskusjonsforum.Models.Thread;


namespace diskusjonsforum.Controllers;


public class CommentController : Controller
{
    //Initialise controllers and interfaces for constructor
    private readonly ICommentRepository _commentRepository;
    private readonly IThreadRepository _threadRepository;
    private readonly ILogger<CommentController> _logger;
    public CommentController(ICommentRepository commentRepository, IThreadRepository threadRepository, ILogger<CommentController> logger)
    {
        _commentRepository = commentRepository;
        _threadRepository = threadRepository;
        _logger = logger;
    }

    //Returns list of all comments
    public List<Comment> GetComments(string cookie)
    {
        List<Comment> comments = _commentRepository.GetAll(cookie);
        return comments;
    }
    
    //Returns Comment/Create view
    [HttpGet("create/{parentCommentId}/{threadId}")] //URL when user replies to a comment or thread 
    public async Task<IActionResult> Create(int? parentCommentId, int threadId)
    {
        if (parentCommentId == 0) {parentCommentId = null;} //If parentCommentId == 0, the comment is a direct reply to the thread

        try
        {
            var cookie = HttpContext.Request.Cookies["SessionCookie"];
            if (cookie != null)
            {
                Comment parentComment = _commentRepository.GetById(parentCommentId);
                var thread = _threadRepository.GetThreadById(threadId, cookie);
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
                var errormsg = "[CommentController] An error occurred in the Create action. Your cookie might be invalid";
                return RedirectToAction("Error", "Home", new { errormsg });
            }
        }
        catch (Exception ex)
        {
            var errormsg = "[CommentController] An error occurred in the Create action";
            _logger.LogError(ex, "[CommentController] An error occurred in the Create action");
            return RedirectToAction("Error", "Home", new { errormsg });
        }
    }

    //Saves comment submitted through the Create view 
    [HttpPost("save")]
    public async Task<IActionResult> Save(Comment comment)
    {
        var errorMsg = "";
        try
        {
            var cookie = HttpContext.Request.Cookies["SessionCookie"];
            if (cookie != null)
            {
                //Set comment details to current user 
                comment.UserCookie = cookie;

                if (string.IsNullOrWhiteSpace(comment.CommentBody))
                {
                    //Input validation: adds modelerror if submitted CommentBody is empty 
                    ModelState.AddModelError("CommentBody", "Comment can't be empty.");
                    //Recreates the Create-viewmodel and returns a new Comment/Create view
                    var viewModel = new CommentCreateViewModel()
                    {
                        ThreadId = comment.ThreadId,
                        ParentCommentId = comment.ParentCommentId,
                        ParentComment = _commentRepository.GetById(comment.ParentCommentId),
                        Thread = _threadRepository.GetThreadById(comment.ThreadId, cookie)
                    };
                    return View("Create", viewModel);
                }
            }

            if (ModelState.IsValid)
            {
                bool returnOk = await _commentRepository.Add(comment);
                if (returnOk)
                    return RedirectToAction("Thread", "Thread", new { comment.ThreadId }); //Returns the thread if comment submission was successful 
            } else {
                errorMsg = "There was an issue submitting your comment";
                return RedirectToAction("Error", "Home", new { errorMsg });
}
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[CommentController] An error occurred in the Create action");

            // Log the specific exception and its message
            _logger.LogError(ex, "[CommentController] Exception: {0}, Message: {1}", ex.GetType(), ex.Message);

            errorMsg = "Comment creation failed";
            return RedirectToAction("Error", "Home", new { errorMsg });
        }

        // If no other return occurred, return a generic error page or redirect
        errorMsg = "An error occurred while creating the comment.";
        return RedirectToAction("Error", "Home", new { errorMsg = errorMsg });
    }

    [HttpGet("edit/{{commentId}}/{{threadId}}")]
    public IActionResult Edit(int commentId, int threadId)
    {
        try
        {
            var cookie = HttpContext.Request.Cookies["SessionCookie"];
            if (cookie != null)
            {
                Comment commentToEdit = _commentRepository.GetById(commentId);
                Comment parentComment =
                    _commentRepository.GetById(commentToEdit.ParentCommentId); //Throws no exception because parentComment can be null (if user replied directly to thread) 
                Thread thread = _threadRepository.GetThreadById(threadId, cookie);
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
                var errormsg = "[CommentController] An error occurred in the Create action. Your cookie might be invalid";
                return RedirectToAction("Error", "Home", new { errormsg });
            }
        }
        catch (Exception ex) {
            var errormsg = "[CommentController] An error occurred in the Create action";
            _logger.LogError(ex, "[CommentController] An error occurred in the Edit action");
            return RedirectToAction("Error", "Home", new { errormsg });
        }
    }
    [HttpPost("comment/saveEdit")]
    public async Task<IActionResult> SaveEdit(Comment comment)
    {
        var errorMsg = "";
        var cookie = HttpContext.Request.Cookies["SessionCookie"];
        if (cookie != null)
        {
            
            var uneditedComment = _commentRepository.GetById(comment.CommentId);
            _commentRepository.DetachEntity(uneditedComment); // Detach the comment since there are two instances of the same comment.
            
            // Add custom validation for the thread content
            if (string.IsNullOrWhiteSpace(comment.CommentBody) || string.IsNullOrWhiteSpace(comment.CommentBody))
            {
                // Content is empty, add a model error
                ModelState.AddModelError("CommentBody", "Comment content is required.");
                var viewModel = new CommentCreateViewModel()
                {
                    ThreadId = uneditedComment.ThreadId,
                    ParentCommentId = uneditedComment.ParentCommentId,
                    ParentComment = uneditedComment.ParentComment,
                    CommentToEdit = _commentRepository.GetById(comment.CommentId),
                    Thread = uneditedComment.Thread
                };
                // Gets thread categories and passes them to View. Used to generate dropdown list of available thread categories 
                return View("Edit", viewModel);
            }
            
            //Reconstruct the comment before saving 
            comment.UserCookie = uneditedComment.UserCookie;
            comment.CommentLastEditedAt = DateTime.Now;
            comment.ThreadId = uneditedComment.ThreadId;
            comment.ParentCommentId = uneditedComment.ParentCommentId;
            comment.CommentCreatedAt = uneditedComment.CommentCreatedAt;
            //Checks if user is owner before editing
            if (comment.UserCookie != cookie)
            {
                errorMsg = "Could not verify that you are the owner of the Thread";
                return RedirectToAction("Error", "Home", new { errorMsg });
            }
            if (ModelState.IsValid)
            {
                bool returnOk = await _commentRepository.Update(comment);
                if (returnOk)
                    return RedirectToAction("Thread", "Thread", new {comment.ThreadId});
            }
        }
        
        _logger.LogWarning("[CommentController] Comment edit failed {@comment}", comment);
        errorMsg = "Could not save your edits";
        return RedirectToAction("Error", "Home", new {errorMsg});
    }

    public async Task<IActionResult> DeleteComment(int commentId)
    {
        Comment comment = _commentRepository.GetById(commentId);

        try
        {
            // Checks if the user is the owner of the comment before deleting
            var cookie = HttpContext.Request.Cookies["SessionCookie"];
            if (comment.UserCookie == cookie)
            {
                List<Comment> childcomments = AddChildren(comment, cookie);

                foreach (var child in childcomments)
                {
                    //Deletes all child comments and saves changes
                    _commentRepository.Remove(child);
                    await _commentRepository.SaveChangesAsync();
                }

                _commentRepository.Remove(comment);
                await _commentRepository.SaveChangesAsync();

                return RedirectToAction("Thread", "Thread", new { comment.ThreadId });
            }
            else
            {
                var errorMsg = "You do not have permission to delete this comment.";
                _logger.LogError(errorMsg);
                return RedirectToAction("Error", "Home", new { errorMsg });
            }
        }
        catch (Exception ex)
        {
            var errorMsg = "An error occurred while deleting the comment.";
            _logger.LogError(ex, "[CommentController] An error occurred in the DeleteComment action.");
            return RedirectToAction("Error", "Home", new { errorMsg });
        }
    }

    //recursively finds all replies to the comment 
    private List<Comment> AddChildren(Comment parentComment, string cookie)
    {
        List<Comment> newChildren = _commentRepository.GetChildren(parentComment, cookie);
        List<Comment> newerChildren = new List<Comment>();
        foreach (Comment child in newChildren)
        {
            newerChildren.AddRange(AddChildren(child, cookie));
        }
        newChildren.AddRange(newerChildren);
        return newChildren;
    }
}

