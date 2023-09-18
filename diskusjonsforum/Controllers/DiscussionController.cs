using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using diskusjonsforum.Models;
using diskusjonsforum.ViewModels;
//using diskusjonsforum.ViewModels; //Kan slettes hvis vi ikke lager ViewModels

namespace diskusjonsforum.Controllers;


public class DiscussionController : Controller
{
    private readonly DiscussionDbContext _discussionDbContext;

    public DiscussionController(DiscussionDbContext discussionDbContext)
    {
        _discussionDbContext = discussionDbContext;
    }

    public IActionResult Table()
    {
        List<Discussion> discussions = _discussionDbContext.Discussions.ToList();
        var discussionListViewModel = new DiscussionListViewModel(discussions, "Table");
        return View(discussionListViewModel);
    }

    public List<Discussion> GetDiscussionThreads()
    {
        var discussionThreads = new List<Discussion>();
        var thread1 = new Discussion
        {
            Category = "Category1",
            Description = "test descirption",
            Title = "Hei"
        };
        discussionThreads.Add(thread1);
        return discussionThreads;
    }
}

    //public IActionResult ListView()
    //{
    //    //Henter "DiscussionThreads" fra DB og legger til liste
    //    List<DiscussionThread> threads = _discussionDbContext.DiscussionThreads.ToList();
    //    var discussionListViewModel = new DiscussionListViewModel(discussions, "List");
    //    return View(discussionListViewModel);
    //}
