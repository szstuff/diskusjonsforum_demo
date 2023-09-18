using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using diskusjonsforum.Models;
using diskusjonsforum.ViewModels;
//using diskusjonsforum.ViewModels; //Kan slettes hvis vi ikke lager ViewModels

namespace diskusjonsforum.Controllers

{
    public class DiscussionThreadController : Controller
    {
        //private readonly DiscussionDbContext _discussionDbContext;

        //public DiscussionController(DiscussionDbContext discussionDbContext)
        //{
        //    _discussionDbContext = discussionDbContext;
        //} Tror dette kan slettes?

        public IActionResult Table()
        {
            var discussionThreads = GetDiscussionThreads();
            var discussionListViewModel = new DiscussionListViewModel(discussionThreads, "Table");
            return View(discussionListViewModel);
        }

        public List<DiscussionThread> GetDiscussionThreads()
        {
            var discussionThreads = new List<DiscussionThread>();
            var thread1 = new DiscussionThread
            {
                Category = "Category1",
                Description = "test descirption",
                Title = "Hei"
            };
            discussionThreads.Add(thread1);
            return discussionThreads;
        }

        //public IActionResult ListView()
        //{
        //    //Henter "DiscussionThreads" fra DB og legger til liste
        //    List<DiscussionThread> threads = _discussionDbContext.DiscussionThreads.ToList();
        //    var discussionListViewModel = new DiscussionListViewModel(discussions, "List");
        //    return View(discussionListViewModel);
        //}
    }
}