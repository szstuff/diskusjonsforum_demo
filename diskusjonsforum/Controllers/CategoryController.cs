using Diskusjonsforum.Models;
using diskusjonsforum.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Thread = Diskusjonsforum.Models.Thread;

namespace diskusjonsforum.Controllers;

public class CategoryController : Controller
{
    
    private readonly ThreadDbContext _threadDbContext;

    public CategoryController(ThreadDbContext threadDbContext)
    {
        _threadDbContext = threadDbContext;
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
        List<Category> categories = _threadDbContext.Categories.Include(category => category.ThreadsInCategory)
            .ToList();

        
        foreach (var category in categories)
        {
            category.ThreadsInCategory.AddRange(GetThreads(category));
        }
        return View();
    }

    public IEnumerable<Thread> GetThreads(Category category)
    {
        return _threadDbContext.Threads.Where(thread => thread.ThreadCategory == category);
    }

    public IActionResult Category(string name)
    {
        var category = _threadDbContext.Categories.Include(c => c.ThreadsInCategory)!
            .FirstOrDefault(c => c.CategoryName == name);
       

        if (category == null)
        {
            return NotFound();
        }
        
        return View(category);

    }
}