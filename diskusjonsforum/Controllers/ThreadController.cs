using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Diskusjonsforum.Models;
using diskusjonsforum.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging; // Add this namespace
using System;
using System.Linq;
using System.Collections.Generic;
using static Diskusjonsforum.Models.Category;
using Thread = Diskusjonsforum.Models.Thread;
using Microsoft.VisualStudio.Text.Tagging;

namespace diskusjonsforum.Controllers;


public class ThreadController : Controller
{
    private readonly ThreadDbContext _threadDbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<ThreadController> _logger;

    public ThreadController(ThreadDbContext threadDbContext, UserManager<ApplicationUser> userManager, ILogger<ThreadController> logger)
    {
        _threadDbContext = threadDbContext;
        _userManager = userManager;
        _logger = logger;
    }

    public IActionResult Table()
    {
        try
        {
            List<Thread> threads = _threadDbContext.Threads.Include(t => t.User)
                .Include(thread => thread.ThreadComments)
                .Include(thread => thread.ThreadCategory)
                .ToList();
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
        return _threadDbContext.Comments.Where(comment => comment.ThreadId == thread.ThreadId);
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
            var thread = _threadDbContext.Threads.Include(t => t.ThreadComments).ThenInclude(t => t.User)
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "[ThreadController] An error occurred in the Thread action for thread ID: {0}", threadId);
            // Handle the error or return an error response if needed.
            return RedirectToAction("Error", "Home", new { errorMsg = "An error occurred while loading the thread." });
        }

    }

    //100% chatGPT, vi burde kanskje se om vi kan finne artikler på nett med samme struktur for kilde
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
    // Slutt på ren chat-gpt

    [HttpGet]
    public IActionResult Create()
    {
        try
        {
            if (HttpContext.User.Identity!.IsAuthenticated)
            {
                // Gets thread categories and passes them to View. Used to generate dropdown list of available thread categories 
                var categories = _threadDbContext.Categories.ToList(); // Fetch all categories from the database.
                ViewBag.Categories = new SelectList(categories, "CategoryName", "CategoryName");
            }
            else
            {
                // Redirects to the login page if the user is not logged in
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[ThreadController] An error occurred in the Create method.");
            return View("Error");
        }
    }

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

                try
                {
                    if (ModelState.IsValid)
                    {
                        _threadDbContext.Threads.Add(thread);
                        _threadDbContext.SaveChanges();
                        return RedirectToAction(nameof(Table));
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
                errorMsg = "Could not create your thread because there was an issue authenticating you";
                return RedirectToAction("Error", "Home", new { errorMsg });
            }
        }
        else
        {
            _logger.LogWarning("[ThreadController] User is not authenticated in the Create action.");
            return View(thread);
        }
    }

    [HttpGet("edit/{threadId}")]
    public async Task<IActionResult> Edit(int threadId)
    {
        try
        {
            if (HttpContext.User.Identity!.IsAuthenticated)
            {
                Thread threadToEdit = _threadDbContext.Threads.FirstOrDefault(t => t.ThreadId == threadId) ??
                                      throw new InvalidOperationException("Requested thread not found. commentId:" + threadId);
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
            _logger.LogError(ex, "[ThreadController] An error occurred in the Edit method.");
            return RedirectToAction("Error", "Home", new { errorMsg });
        }
    }

    public async Task<IActionResult> SaveEdit(Thread thread)
    {
        var errorMsg = "";
        try
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                ModelState.Remove("User"); // Workaround for invalid model state. The model isn't really invalid, but it was evaluated BEFORE the controller added User and UserId. Therefore the validity of the "User" key can be removed.

                // Checks if the user is the owner or admin before editing
                var userRoles = await _userManager.GetRolesAsync(user);
                if (user.Id != thread.UserId || !userRoles.Contains("Admin"))
                {
                    errorMsg = "Could not verify that you are the owner of the Thread";
                    _logger.LogError(errorMsg);
                    return RedirectToAction("Error", "Home", new { errorMsg });
                }

                if (ModelState.IsValid)
                {
                    _threadDbContext.Threads.Update(thread);
                    await _threadDbContext.SaveChangesAsync();
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
        Thread thread = _threadDbContext.Threads.FirstOrDefault(t => t.ThreadId == threadId) ??
                        throw new InvalidOperationException("Requested Thread not found. ThreadId: " + threadId);

        var user = await _userManager.GetUserAsync(HttpContext.User);
        var userRoles = await _userManager.GetRolesAsync(user);
        var errorMsg = "";

        try
        {
            if (thread.UserId != user.Id || !userRoles.Contains("Admin"))
            {
                errorMsg = "Could not verify that you are the owner of the Thread";
                return RedirectToAction("Error", "Home", new { errorMsg });
            }

            _threadDbContext.Threads.Remove(thread);
            _threadDbContext.SaveChanges();
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
