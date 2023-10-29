using diskusjonsforum.DAL;
using Microsoft.AspNetCore.Mvc;
using diskusjonsforum.ViewModels;
using Thread = Diskusjonsforum.Models.Thread;

namespace diskusjonsforum.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IThreadRepository _threadRepository;

        public HomeController(ILogger<HomeController> logger, IThreadRepository threadRepository)
        {
            _logger = logger;
            _threadRepository = threadRepository;
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

        public IEnumerable<Thread> GetThreads()
        {
            try
            {
                var threads = _threadRepository.GetAll();
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

