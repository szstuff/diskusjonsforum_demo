using Diskusjonsforum.Models;
using Microsoft.EntityFrameworkCore;
using Thread = System.Threading.Thread;

namespace diskusjonsforum.DAL;

public class CategoryRepository : ICategoryRepository
{
    private readonly ThreadDbContext _threadDbContext;

    public CategoryRepository(ThreadDbContext context)
    {
        _threadDbContext = context;
    }

    public List<Category> GetCategories()
    {
        return _threadDbContext.Categories
            .Include(c => c.ThreadsInCategory)
            .ToList();
    }
    
    public List<Diskusjonsforum.Models.Thread> GetThreads(Category category)
    {
        return _threadDbContext.Threads
            .Where(thread => thread.Category == category)
            .ToList();
    }

    public Category GetCategoryByName(String name)
    {
        return _threadDbContext.Categories
            .Include(c => c.ThreadsInCategory)
            .FirstOrDefault(c => c.CategoryName == name);
    }

    public IQueryable<Diskusjonsforum.Models.Thread> GetThreadsByCategory(Category category)
    {
        return _threadDbContext.Threads.Where(thread => thread.Category == category);
    }
}
