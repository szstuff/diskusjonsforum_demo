using Microsoft.AspNetCore.Mvc;
using Diskusjonsforum.Models;
using diskusjonsforum.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
// Add this namespace
using diskusjonsforum.DAL;
using Thread = Diskusjonsforum.Models.Thread;

namespace diskusjonsforum.Controllers;


public class ThreadController : Controller
{
    private readonly IThreadRepository _threadRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<ThreadController> _logger;

    public ThreadController(IThreadRepository threadDbContext, ICategoryRepository categoryRepository, ICommentRepository commentRepository, UserManager<ApplicationUser> userManager, ILogger<ThreadController> logger)
    {
        _threadRepository = threadDbContext;
        _categoryRepository = categoryRepository;
        _commentRepository = commentRepository;
        _userManager = userManager;
        _logger = logger;
    }

    public IActionResult Table()
    {
        try
        {
            var threads = _threadRepository.GetAll();
            foreach (var thread in threads)
            {
                thread.ThreadComments.AddRange(GetComments(thread));
            }
            var threadListViewModel = new ThreadListViewModel(threads, "Table");
            return View(threadListViewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[ThreadController] An error occurred in the Table action.");
            return RedirectToAction("Error", "Home", new { errorMsg = "An error occurred while loading threads." });
        }
    }

    public IQueryable<Comment> GetComments(Thread thread)
    {
        return _commentRepository.GetThreadComments(thread);
    }

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
            
            return new List<Thread>();
        }
    }

    public IActionResult Thread(int threadId)
    {
        try
        {
            var thread = _threadRepository.GetThreadById(threadId);

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
            // Handle the error or return an error response if needed.
            return RedirectToAction("Error", "Home", new { errorMsg = "An error occurred while loading the thread." });
        }

    }
    
    public List<Comment> SortComments(List<Comment> comments)
    {
        try
        {
            var sortedComments = new List<Comment>();

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
            // Handle the error or return an error response if needed.
            return new List<Comment>();
        }
    }

    private void AddChildComments(Comment parent, List<Comment> allComments, List<Comment> sortedComments)
    {
        try
        {
            var childComments = allComments.Where(c => c.ParentCommentId == parent.CommentId).ToList();
            foreach (var comment in childComments)
            {
                sortedComments.Add(comment);
                AddChildComments(comment, allComments, sortedComments);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[ThreadController] An error occurred in the AddChildComments method.");
            // Handle the error or return an error response if needed.
        }
    }

    [HttpGet]
    public IActionResult Create()
    {
        try
        {
            if (HttpContext.User.Identity!.IsAuthenticated)
            {
                // Gets thread categories and passes them to View. Used to generate dropdown list of available thread categories 
                var categories = _categoryRepository.GetCategories();// Fetch all categories from the database.
                ViewBag.Categories = new SelectList(categories, "CategoryName", "CategoryName");
                return View();

            }
            // Redirects to the login page if the user is not logged in
            return RedirectToPage("/Account/Login", new { area = "Identity" });

        }
        catch (Exception ex)
        {
            var errormsg = "[ThreadController] An error occurred in the Create method.";
            _logger.LogError(ex, "[ThreadController] An error occurred in the Create method.");
            return RedirectToAction("Error", "Home", new { errormsg });
        }
    }

    [HttpPost]
        public async Task<IActionResult> Create(Thread thread){
        var errorMsg = "";

        if (HttpContext.User.Identity!.IsAuthenticated)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user != null)
            {
                thread.UserId = user.Id;
                thread.User = user;
                ModelState.Remove("User");
                ModelState.Remove("Category");

                // Add custom validation for the thread content
                if (string.IsNullOrWhiteSpace(thread.ThreadBody) || string.IsNullOrWhiteSpace(thread.ThreadTitle))
                {
                    // Content is empty, add a model error
                    ModelState.AddModelError("ThreadContent", "Thread content is required.");
                    // Gets thread categories and passes them to View. Used to generate dropdown list of available thread categories 
                    var categories = _categoryRepository.GetCategories();// Fetch all categories from the database.
                    ViewBag.Categories = new SelectList(categories, "CategoryName", "CategoryName");
                    return View(thread);
                }

                try
                {
                    if (ModelState.IsValid)
                    {
                        bool returnOk = await _threadRepository.Add(thread);
                        if (returnOk)
                        {
                            return RedirectToAction(nameof(Table));
                        }
                    }
                    else
                    {
                        errorMsg = "Could not create your thread because there was an issue validating its content";
                        return RedirectToAction("Error", "Home", new { errorMsg });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[ThreadController] Error creating a thread: {0}", thread.ThreadTitle);
                    errorMsg = "Could not create your thread due to a database error.";
                    return RedirectToAction("Error", "Home", new { errorMsg });
                }
            }
            else
            {
                _logger.LogError("[ThreadController] Error authenticating the user in the Create action.");
                errorMsg = "Could not create your thread because there was an issue authenticating you. Please log out and in, and try again";
                return RedirectToAction("Error", "Home", new { errorMsg });
            }
        }
        else
        {
            _logger.LogWarning("[ThreadController] User is not authenticated in the Create action.");
            return View(thread);
        }

        return null;
        }

    [HttpGet("edit/{threadId}")]
    public async Task<IActionResult> Edit(int threadId)
    {
        try
        {
            if (HttpContext.User.Identity!.IsAuthenticated)
            {
                // Gets thread categories and passes them to View. Used to generate dropdown list of available thread categories 
                var categories = _categoryRepository.GetCategories(); //Fetch all categories from the database.
                ViewBag.Categories = new SelectList(categories, "CategoryName", "CategoryName");

                Thread threadToEdit = _threadRepository.GetThreadById(threadId);
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var userIsAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                if (user.Id == threadToEdit.UserId || userIsAdmin)
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

    public async Task<IActionResult> SaveEdit(Thread thread)
    {
        string errorMsg = "An error occured when trying to save your edit";
        try
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                ModelState.Remove("User");

                // Checks if the user is the owner or admin before editing
                var userRoles = await _userManager.GetRolesAsync(user);
                if (user.Id != thread.UserId && !userRoles.Contains("Admin"))
                {
                    errorMsg = "Could not verify that you are the owner of the Thread";
                    _logger.LogError(errorMsg);
                    return RedirectToAction("Error", "Home", new { errorMsg });
                }
                ModelState.Remove("Category");
                if (ModelState.IsValid)
                {
                    await _threadRepository.Update(thread);
                    await _threadRepository.SaveChangesAsync();
                    return RedirectToAction("Thread", "Thread", new { thread.ThreadId });
                }
            }

            errorMsg = "Error occurred when saving the changes you made to the thread";
            _logger.LogError(errorMsg);
            return RedirectToAction("Error", "Home", new { errorMsg });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[ThreadController] An error occurred in the SaveEdit method.");
            return RedirectToAction("Error", "Home", new { errorMsg });
        }
    }

    public async Task<IActionResult> DeleteThread(int threadId)
    {
        Thread thread = _threadRepository.GetThreadById(threadId);

        var user = await _userManager.GetUserAsync(HttpContext.User);
        var userRoles = await _userManager.GetRolesAsync(user);
        string errorMsg = "";

        try
        {
            if (thread.UserId != user.Id && !userRoles.Contains("Admin"))
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



}
