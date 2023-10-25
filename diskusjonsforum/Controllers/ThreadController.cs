using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Diskusjonsforum.Models;
using diskusjonsforum.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Diskusjonsforum.Models.Category;
using Thread = Diskusjonsforum.Models.Thread;


namespace diskusjonsforum.Controllers;


public class ThreadController : Controller
{
    private readonly ThreadDbContext _threadDbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public ThreadController(ThreadDbContext threadDbContext, UserManager<ApplicationUser> userManager)
    {
        _threadDbContext = threadDbContext;
        _userManager = userManager;
    }

    public IActionResult Table()
    {
        List<Thread>
            threads = _threadDbContext.Threads.Include(t => t.User).Include(thread => thread.ThreadComments).Include(thread => thread.ThreadCategory)
                .ToList(); //.Include her gjør "Eager Loading". Laster inn Users tabellen for å kunne vise username
        foreach (var thread in threads)
        {
            thread.ThreadComments.AddRange(GetComments(thread));
        }
        var threadListViewModel = new ThreadListViewModel(threads,  "Table");
        return View(threadListViewModel);
    }

    public IQueryable<Comment> GetComments(Thread thread)
    {
        return _threadDbContext.Comments.Where(comment => comment.ThreadId == thread.ThreadId);
    }

    public List<Thread> GetThreads()
    {
        var threads = new List<Thread>();
        return threads;
    }

    public IActionResult Thread(int threadId)
    {
        var thread = _threadDbContext.Threads.Include(t => t.ThreadComments)!.ThenInclude(t => t.User)
            .FirstOrDefault(t => t.ThreadId == threadId);
        _threadDbContext.Entry(thread)
            .Reference(t => t!.User)
            .Load();

        if (thread == null)
        {
            return NotFound();
        }

        thread.ThreadComments = SortComments(thread.ThreadComments!);

        return View(thread);

    }

    //100% chatGPT, vi burde kanskje se om vi kan finne artikler på nett med samme struktur for kilde
    public List<Comment> SortComments(List<Comment> comments)
    {
        var sortedComments = new List<Comment>();

        foreach (var comment in comments.Where(c => c.ParentCommentId == null))
        {
            sortedComments.Add(comment);
            AddChildComments(comment, comments, sortedComments);
        }

        return sortedComments;
    }

    private void AddChildComments(Comment parent, List<Comment> allComments, List<Comment> sortedComments)
    {
        var childComments = allComments.Where(c => c.ParentCommentId == parent.CommentId).ToList();
        foreach (var comment in childComments)
        {
            sortedComments.Add(comment);
            AddChildComments(comment, allComments, sortedComments);
        }
    }
    // Slutt på ren chat-gpt

    [HttpGet]
    public IActionResult Create()
    {
        if (HttpContext.User.Identity!.IsAuthenticated)
        {
            //Gets thread categories and passes them to View. Used to generate dropdown list of available thread categories 
            var categories = _threadDbContext.Categories.ToList(); // Fetch all categories from the database.
            ViewBag.Categories = new SelectList(categories, "CategoryName", "CategoryName");
        } else {
            //Redirects to login page if user not logged in
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Thread thread)
    {
        var errorMsg = "";
        if (HttpContext.User.Identity!.IsAuthenticated)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user != null)
            {
                thread.UserId = user.Id;
                thread.User = user;
                ModelState.Remove("User");

                if (ModelState.IsValid)
                {
                    _threadDbContext.Threads.Add(thread);
                    _threadDbContext.SaveChanges(); // or await _threadDbContext.SaveChangesAsync(); for async
                    
                    return RedirectToAction(nameof(Table));
                }
                else
                {
                    errorMsg = "Could not create your thread because there was an issue validating it's content";
                    return RedirectToAction("Error", "Home", new { errorMsg });
                }
            } else
            {
                errorMsg = "Could not create your thread because there was an issue authenticating you";
                return RedirectToAction("Error", "Home", new { errorMsg });

            }
        }

        return View(thread);
    }

    [HttpGet("edit/{threadId}")]
    public async Task<IActionResult> Edit(int threadId)
    {
        if (HttpContext.User.Identity!.IsAuthenticated)
        {
            Thread threadToEdit = _threadDbContext.Threads.FirstOrDefault(t => t.ThreadId == threadId) ??
                                  throw new InvalidOperationException("Requested thread not found. commentId:" +
                                                                      threadId);
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userIsAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            if (user.Id == threadToEdit.UserId || userIsAdmin)
            {
                return View(threadToEdit);

            } else {
                //Redirects to login page if user not logged in
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            }
        }

        var errorMsg = "Error when trying to load Edit Thread view";
        return RedirectToAction("Error", "Home", new { errorMsg });
    }

    public async Task<IActionResult> SaveEdit(Thread thread)
    {
        var errorMsg = "";
        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user != null)
        {
            ModelState.Remove(
                "User"); //Workaround for invalid modelstate. The model isnt really invalid, but it was evaluated BEFORE the controller added User and UserId. Therefore the validty of the "User" key can be removed 

            //Checks if user is owner or admin before editing
            var userRoles = await _userManager.GetRolesAsync(user);
            if (user.Id != thread.UserId || !userRoles.Contains("Admin"))
            {
                errorMsg = "Could not verify that you are the owner of the Thread";
                return RedirectToAction("Error", "Home", new { errorMsg });
            }

            if (ModelState.IsValid)
            {
                _threadDbContext.Threads.Update(thread);
                await _threadDbContext.SaveChangesAsync();
                return RedirectToAction("Thread", "Thread", new { thread.ThreadId });
            }
        }

        errorMsg = "Error occured when saving the changes you made to the thread";
        return RedirectToAction("Error", "Home", new { errorMsg });

    }

    public async Task<IActionResult> DeleteThread(int threadId)
    {

        Thread thread = _threadDbContext.Threads.FirstOrDefault(t => t.ThreadId == threadId)?? throw new InvalidOperationException("Requested Thread not found. ThreadId: " + threadId);
        //Checks if user is owner or admin before editing
        var user = await _userManager.GetUserAsync(HttpContext.User);
        var userRoles = await _userManager.GetRolesAsync(user);
        if (thread.UserId != user.Id || !userRoles.Contains("Admin"))
        {
            var userAdmin = userRoles.Contains("Admin");
            var errorMsg = "Could not verify that you are the owner of the Thread";
            return RedirectToAction("Error", "Home", new { errorMsg });
        }

        _threadDbContext.Threads.Remove(thread);
        await _threadDbContext.SaveChangesAsync();
        return RedirectToAction("Table", "Thread");
    }


}
