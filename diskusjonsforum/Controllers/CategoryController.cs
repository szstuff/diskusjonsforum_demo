using Diskusjonsforum.Models;
using Microsoft.AspNetCore.Mvc;
using diskusjonsforum.DAL;
using Thread = Diskusjonsforum.Models.Thread;

namespace diskusjonsforum.Controllers;

public class CategoryController : Controller
{
    
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILogger<CategoryController> _logger; //Adds logger

    public CategoryController(ICategoryRepository categoryRepository, ILogger<CategoryController> logger)
    {
        _categoryRepository = categoryRepository;
        _logger = logger;
    }
    
    // public IActionResult Table()
    // {
    //     List<Category> categories = _threadDbContext.Categories.Include<>(t => t.CategoryName)
    //             .ToList(); 
    //     
    //     foreach (var category in categories)
    //     {
    //         category.ThreadsInCategory.Append<>(GetThreadCount(category));
    //     }
    //     var categoryListViewModel = new CategoryListViewModel(categories, thread);
    //     return View(categoryListViewModel);
    // }
    
    public IActionResult Table()
    {
        try
        {
            var categories = _categoryRepository.GetCategories();
        
            foreach (var category in categories)
            {
                if (category.ThreadsInCategory != null)
                {
                    category.ThreadsInCategory.AddRange(GetThreads(category));
                }else {
                    var errormsg = "[CategoryController] An error occurred in the Table action: category threads are null";
                    _logger.LogError("[CategoryController] An error occurred in the Table action: category threads are null");
                    return RedirectToAction("Error", "Home", new { errormsg });
                }
            } 
           
        return View();

        }
        catch (Exception ex)
        {
            //Log exception
            _logger.LogError(ex, "An error occured in the Table action.");

            //Handles error and returns error view/msg
            var errormsg = "[CategoryController] An error occurred in the Table action";
            _logger.LogError(ex, "[CategoryController] An error occurred in the Table action");
            return RedirectToAction("Error", "Home", new { errormsg });
        }

    }

    public IEnumerable<Thread> GetThreads(Category category)
    {
        return _categoryRepository.GetThreadsByCategory(category);
    }

    public IActionResult Category(Category category)
    {
        try
        {
            category = _categoryRepository.GetCategoryByName(category.CategoryName);


            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        catch (Exception ex)
        {
            //Log exception
            _logger.LogError(ex, "An error occurred in the Category action.");

            //Handles the error and returns error view/msg
            var errormsg = "[CategoryController] An error occurred in the Table action: category threads are null";
            _logger.LogError("[CategoryController] An error occurred in the Table action: category threads are null");
            return RedirectToAction("Error", "Home", new { errormsg });
        }
    }
}