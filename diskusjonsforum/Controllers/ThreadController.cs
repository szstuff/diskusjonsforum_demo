using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using diskusjonsforum.Models;
using diskusjonsforum.ViewModels;
using Thread = diskusjonsforum.Models.Thread;

//using diskusjonsforum.ViewModels; //Kan slettes hvis vi ikke lager ViewModels

namespace diskusjonsforum.Controllers;


public class ThreadController : Controller
{
    private readonly ThreadDbContext _threadDbContext;

    public ThreadController(ThreadDbContext threadDbContext)
    {
        _threadDbContext = threadDbContext;
        //Creates dummy data for testing
        //_threadDbContext.Database.ExecuteSqlRaw("insert INTO Users (Name, PasswordHash, Email, Administrator) VALUES (\"stilian\", \"pass\", \"email@email.com\", True)\n");
        //_threadDbContext.Database.ExecuteSqlRaw("insert into Threads (Title, Description, Category, CreatedAt, CreatedBy) VALUES (\"Hei\", \"Heihiehiehue\", \"Hei\", \"2020-09-10\", 1)\n");
        //_threadDbContext.Database.ExecuteSqlRaw("insert into Comments (Body, CreatedAt, Thread, User, ParentCommentId) values (\"Hei1\", \"2020-09-10\", 1, 1, null) ");
        //_threadDbContext.Database.ExecuteSqlRaw("insert into Comments (Body, CreatedAt, Thread, User, ParentCommentId) values (\"HeiHei2\", \"2020-09-10\", 1, 1, 1) ");
        //_threadDbContext.Database.ExecuteSqlRaw("insert into Comments (Body, CreatedAt, Thread, User, ParentCommentId) values (\"HeiHeiHei3\", \"2020-09-10\", 1, 1, 2) ");
    }

    public IActionResult Table()
    {
        List<Thread> threads = _threadDbContext.Threads.ToList();
        var threadListViewModel = new ThreadListViewModel(threads, "Table");
        return View(threadListViewModel);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Thread thread)
    {
        if (ModelState.IsValid)
        {
            await _threadRepository.Create(thread);
            return RedirectToAction(nameof(Table);
        }
        return View(thread);
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var thread = await _threadRepository.GetThreadById(id);
        if(thread = null)
        {
            return NotFound();
        }
        return View(thread);
    }

    [HttpPost]
    public async Task<IActionResult> Update(Thread thread)
    {
        if (ModelState.IsValid)
        {
            await _threadRepository.Udate(thread);
            return RedirectToAction(nameof(Table));
        }
        return View(thread);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var thread = await _threadRepository.GetThreadById(id);
        if (thread == null) 
        { 
            return NotFound(); 
        }
    }

    public List<Thread> GetThreads()
    {
        var threads = new List<Thread>();
        var thread1 = new Thread
        {
            CreatedAt = DateTime.Now,
            Category = "Category1",
            Description = "test descirption",
            Title = "Hei"
        };
        threads.Add(thread1);
        return threads;
    }

    public IActionResult Thread(int threadId)
    {
        var thread = _threadDbContext.Threads.Include(t => t.Comments).FirstOrDefault(t => t.Id == threadId);

        if (thread == null)
        {
            return NotFound();
        }

        thread.Comments = SortComments(thread.Comments);

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
    
}

    //public IActionResult ListView()
    //{
    //    //Henter "Threads" fra DB og legger til liste
    //    List<Thread> threads = _threadDbContext.Threads.ToList();
    //    var threadListViewModel = new ThreadListViewModel(threads, "List");
    //    return View(threadListViewModel);
    //}
