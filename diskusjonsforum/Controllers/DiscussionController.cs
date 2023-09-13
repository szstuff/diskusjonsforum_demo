using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using diskusjonsforum.Models;
namespace diskusjonsforum.Controllers

{
    public class DiscussionController : Controller
    {
        private readonly DiscussionDbContext _discussionDbContext;

        public DiscussionController(DiscussionDbContext _discussionDbContext)
        {
            _discussionDbContext = context;
        }

        public IActionResult Index()
        {
            List<Discussion> discussions = _discussionDbContext.Discussions.ToList();
            var discussions = new ListDiscussionViewModel(discussions, "Table");
            return View(discussions);
        }
    }
