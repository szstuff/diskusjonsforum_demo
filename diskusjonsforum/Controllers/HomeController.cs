using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Diskusjonsforum.Models;
using diskusjonsforum.ViewModels;
using Thread = Diskusjonsforum.Models.Thread;

namespace diskusjonsforum.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ThreadDbContext _threadDbContext;

        public HomeController(ILogger<HomeController> logger, ThreadDbContext threadDbContext)
        {
            _logger = logger;
            _threadDbContext = threadDbContext;
        }

        public IActionResult Index()
        {
            var threads = GetThreads(); // Call your GetThreads method to fetch the list of threads.
            var threadListViewModel = new ThreadListViewModel(threads, "Table")
            {
                Threads = threads
            };

            return View(threadListViewModel);
        }

        public List<Thread> GetThreads()
        {
            try
            {
                var threads = _threadDbContext.Threads
                    .Include(t => t.ThreadComments)
                    .Include(t => t.User) // Include the User navigation property
                    .ToList();

                return threads;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[HomeController] An error occurred in the GetThreads method.");
                return new List<Thread>();
            }
        }

        public IActionResult Error(string errorMsg)
        {
            //Log error message using logger
            _logger.LogError("[HomeController] Error: {0}", errorMsg);
            //View error message in view
            ViewBag.ErrorMsg = errorMsg;
            return View();
        }
    }
}

