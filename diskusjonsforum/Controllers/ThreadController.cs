using Microsoft.AspNetCore.Mvc;
using Diskusjonsforum.Models;
using diskusjonsforum.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
// Add this namespace
using diskusjonsforum.DAL;
using Thread = Diskusjonsforum.Models.Thread;
//The comment below disables certain irrelevant warnings in JetBrains IDE
// ReSharper disable RedundantAssignment

namespace diskusjonsforum.Controllers;


public class ThreadController : Controller
{
    //Initialise controllers and interfaces for constructor
    private readonly IThreadRepository _threadRepository;

    private readonly ICommentRepository _commentRepository;
    private readonly ILogger<ThreadController> _logger;

    public ThreadController(IThreadRepository threadDbContext,
        ICommentRepository commentRepository,
        ILogger<ThreadController> logger)
    {
        _threadRepository = threadDbContext;
        _commentRepository = commentRepository;
        _logger = logger;
    }

    //Returns Thread Table Razor view
    public IActionResult Table()
    {
        var errorMsg = "";
        try
        {
            var cookie = HttpContext.Request.Cookies["SessionCookie"];
            if (cookie == null)
            {
                errorMsg = "Error getting threads. Your cookie is invalid";
                return RedirectToAction("Error", "Home", new { errorMsg });
            }
            
            var threads = _threadRepository.GetAll(cookie);
         
            // Create view model for thread and displays them
            var threadListViewModel = new ThreadListViewModel(threads, "Table");
            ViewBag.CurrentView = "ThreadTable";
            return View(threadListViewModel); 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[ThreadController] An error occurred in the Table action.");
            errorMsg = "An error occurred while loading threads";
            return RedirectToAction("Error", "Home", new { errorMsg });
        }
    }
    
    //Returns comments for a given thread 
    public IQueryable<Comment> GetComments(Thread thread)
    {
        try
        {
            return _commentRepository.GetThreadComments(thread);
        } catch (Exception ex)
        {
            _logger.LogError(ex, "[ThreadController] An error occurred in the GetComments method.");
            return Enumerable.Empty<Comment>().AsQueryable(); //Returns empty collection

        }
    }

    //Returns a list of threads
    public List<Thread> GetThreads()
    {
        try
        {
            var threads = new List<Thread>();
            return threads;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[ThreadController] An error occurred in the GetThreads method.");
            return new List<Thread>(); //Returns empty list 
        }
    }

    public IActionResult Thread(int threadId)
    {
        var cookie = HttpContext.Request.Cookies["SessionCookie"];
        if (cookie == null)
        {
            var errorMsg = "Error getting threads. Your cookie is invalid";
            return RedirectToAction("Error", "Home", new { errorMsg });
        }
        try
        {
            var thread = _threadRepository.GetThreadById(threadId, cookie);

            if (thread == null)
            {
                return NotFound();
            }

            thread.ThreadComments = SortComments(thread.ThreadComments!);

            return View(thread);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[ThreadController] An error occurred in the Thread action for thread ID: {0}", threadId);
            // Redirect to Error view if error occurs
            return RedirectToAction("Error", "Home", new { errorMsg = "An error occurred while loading the thread." });
            
        }

    }
    
    // Sort comments based on their parent-child
    public List<Comment> SortComments(List<Comment> comments)
    {
        try
        {
            var sortedComments = new List<Comment>();
            
            // Go through comments without a parent comment and sorts them
            foreach (var comment in comments.Where(c => c.ParentCommentId == null))
            {
                sortedComments.Add(comment);
                AddChildComments(comment, comments, sortedComments);
            }

            return sortedComments;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[ThreadController] An error occurred in the SortComments method.");
            return new List<Comment>();
        }
    }

    // Add child comment to sortedComments list
    private void AddChildComments(Comment parent, List<Comment> allComments, List<Comment> sortedComments)
    {
        try
        {
            // Find child comments for respective parent comment and add them to the sorted comment list
            var childComments = allComments.Where(c => c.ParentCommentId == parent.CommentId).ToList();
            foreach (var comment in childComments)
            {
                sortedComments.Add(comment);
                AddChildComments(comment, allComments, sortedComments); // Find children of this child comment
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[ThreadController] An error occurred in the AddChildComments method.");
        }
    }

    // If user is authenticated, retrieve thread categories from repository to the view
    [HttpGet]
    public IActionResult Create()
    {
        try
        {
            var cookie = HttpContext.Request.Cookies["SessionCookie"];
            if (cookie != null)
            {
                return View();

            }
            var errormsg = "[ThreadController] An error occurred in the Create method. Your cookie is invalid";
            return RedirectToAction("Error", "Home", new { errormsg });

        }
        catch (Exception ex)
        {
            var errormsg = "[ThreadController] An error occurred in the Create method.";
            _logger.LogError(ex, "[ThreadController] An error occurred in the Create method.");
            return RedirectToAction("Error", "Home", new { errormsg });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(Thread thread)
    {
        var errorMsg = "";

        var cookie = HttpContext.Request.Cookies["SessionCookie"];
        if (cookie != null) // Check cookie is made
        {
            thread.UserCookie = cookie;
            ModelState.Remove("Category");

            // Add custom validation for the thread content
            if (string.IsNullOrWhiteSpace(thread.ThreadBody) || string.IsNullOrWhiteSpace(thread.ThreadTitle))
            {
                // Content is empty, add a model error
                ModelState.AddModelError("ThreadContent", "Thread content is required.");
                return View(thread);
            }

            try
            {
                // If the  model is valid, add the thread
                if (ModelState.IsValid)
                {
                    bool returnOk = await _threadRepository.Add(thread);
                    if (returnOk)
                    {
                        return RedirectToAction(nameof(Table));
                    }
                }
                errorMsg = "Could not create your thread because there was an issue validating its content";
                return RedirectToAction("Error", "Home", new { errorMsg });
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[ThreadController] Error creating a thread: {0}", thread.ThreadTitle);
                errorMsg = "Could not create your thread due to a database error.";
                return RedirectToAction("Error", "Home", new { errorMsg });
            }
        }
    
        _logger.LogError("[ThreadController] UserCookie is null.");
        errorMsg = "An error occured when loading the page. Your cookie is invalid";
        return RedirectToAction("Error", "Home", new { errorMsg });
        
    }

    [HttpGet("edit/{threadId}")]
    public async Task<IActionResult> Edit(int threadId)
    {
        try
        {
            var cookie = HttpContext.Request.Cookies["SessionCookie"];
            if (cookie != null) // Check cookie is made
            {
                Thread threadToEdit = _threadRepository.GetThreadById(threadId, cookie);  // Retrieve thread to edit with threadId
                
                // Checks if the user is the owner or admin before allowing to edit
                if (cookie == threadToEdit.UserCookie) 
                {
                    return View(threadToEdit);
                }
                else
                {
                    // Redirects to login page if the user is not logged in
                    return RedirectToPage("/Account/Login", new { area = "Identity" });
                }
            }

            var errorMsg = "Error when trying to load Edit Thread view";
            _logger.LogError("[ThreadController] An error occurred in the Edit method.");
            return RedirectToAction("Error", "Home", new { errorMsg });
        }
        catch (Exception ex)
        {
            var errorMsg = "[ThreadController] An error occurred in the Edit method.";
            _logger.LogError(ex, "[ThreadController] An error occurred in the Edit method.");
            return RedirectToAction("Error", "Home", new { errorMsg });
        }
    }

    [HttpPost]
    // Saves edits made to a thread
    public async Task<IActionResult> SaveEdit(Thread thread)
    {
        var errorMsg = "An error occured when trying to save your edit";
        try
        {
            var cookie = HttpContext.Request.Cookies["SessionCookie"];
            if (cookie != null) // Check cookie is made
            {
                ModelState.Remove("User"); //Workaround for invalid modelstate. The model isnt really invalid, but it was evaluated BEFORE the controller added User and UserId. Therefore the validty of the "User" key can be removed

                // Add custom validation for the thread content
                if (string.IsNullOrWhiteSpace(thread.ThreadBody) || string.IsNullOrWhiteSpace(thread.ThreadTitle))
                {
                    // Content is empty, add a model error
                    ModelState.AddModelError("ThreadContent", "Thread content is required.");
                    Thread threadToEdit = _threadRepository.GetThreadById(thread.ThreadId, cookie);
                    return View("Edit", threadToEdit);
                }
                
                if (cookie != thread.UserCookie)
                {
                    errorMsg = "Could not verify that you are the owner of the Thread";
                    _logger.LogError(errorMsg);
                    return RedirectToAction("Error", "Home", new { errorMsg });
                }

                try
                {
                    if (ModelState.IsValid)
                    {
                        bool returnOk = await _threadRepository.Update(thread);
                        if (returnOk)
                            return RedirectToAction("Thread", "Thread", new { thread.ThreadId });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[ThreadController] Error editing thread: {0}", thread.ThreadId);
                    errorMsg = "Could not edit your thread due to a database error.";
                    return RedirectToAction("Error", "Home", new { errorMsg });
                }
            }

            _logger.LogError("[ThreadController] Error occurred when saving the changes you made to the thread");
            errorMsg = "Error occurred when saving the changes you made to the thread";
            return RedirectToAction("Error", "Home", new { errorMsg });
        }
        catch (Exception ex)
        {
            errorMsg = "Error occurred when saving the changes you made to the thread";
            _logger.LogError(ex, "[ThreadController] An error occurred in the SaveEdit method.");
            return RedirectToAction("Error", "Home", new { errorMsg });
        }
    }
    
    // Delete thread with given threadId if user has permission
    public async Task<IActionResult> DeleteThread(int threadId)
    {
        var cookie = HttpContext.Request.Cookies["SessionCookie"];
        string errorMsg = "";
        if (cookie == null)
        {
            errorMsg = "Error getting threads. Your cookie is invalid";
            return RedirectToAction("Error", "Home", new { errorMsg });
        }
        Thread thread = _threadRepository.GetThreadById(threadId, cookie);
        
        // Checks if the user is either the owner of the comment or an admin before deleting
        try
        {
            if (thread.UserCookie != cookie) //If user is admin or owner
            {
                errorMsg = "You do not have permission to delete this thread.";
                return RedirectToAction("Error", "Home", new { errorMsg });
            }

            await _threadRepository.Remove(thread);
            await _threadRepository.SaveChangesAsync();
            return RedirectToAction("Table", "Thread");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[ThreadController] Error deleting thread with ID: {0}", threadId);
            errorMsg = "An error occurred while deleting the thread.";
            return RedirectToAction("Error", "Home", new { errorMsg });
        }
    }
    
    // Search for posts in the database with provided search query
    public IActionResult SearchPosts(string searchQuery)
    {
        var cookie = HttpContext.Request.Cookies["SessionCookie"];
        if (cookie == null)
        {
            var errorMsg = "Error getting threads. Your cookie is invalid";
            return RedirectToAction("Error", "Home", new { errorMsg });
        }
        
        var threads = _threadRepository.GetAll(cookie);
        
        // Checks if whatever the user is typing exists
        if (!string.IsNullOrEmpty(searchQuery))
        {
            threads = threads.Where(t => t.ThreadTitle.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // Create an object with both threadTitle and threadId
        var searchResults = threads.Select(t => new { threadTitle = t.ThreadTitle, threadId = t.ThreadId });

        // Return the search results as JSON
        return Json(searchResults);
    }



}
