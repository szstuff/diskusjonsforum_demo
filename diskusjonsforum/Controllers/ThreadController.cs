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
    }

    public IActionResult Table()
    {
        List<Thread> threads = _threadDbContext.Threads.ToList();
        var threadListViewModel = new ThreadListViewModel(threads, "Table");
        return View(threadListViewModel);
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
}

    //public IActionResult ListView()
    //{
    //    //Henter "Threads" fra DB og legger til liste
    //    List<Thread> threads = _threadDbContext.Threads.ToList();
    //    var threadListViewModel = new ThreadListViewModel(threads, "List");
    //    return View(threadListViewModel);
    //}
