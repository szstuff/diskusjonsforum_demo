using Diskusjonsforum.Models;
using Microsoft.EntityFrameworkCore;
using Thread = Diskusjonsforum.Models.Thread;

namespace diskusjonsforum.DAL;

public class ThreadRepository : IThreadRepository
{
    private readonly ThreadDbContext _threadDbContext;
    private readonly ILogger<ThreadRepository> _logger;

    public ThreadRepository(ThreadDbContext threadDbContext, ILogger<ThreadRepository> logger)
    {
        _threadDbContext = threadDbContext;
        _logger = logger;
    }

    public IEnumerable<Thread> GetAll()
    {
        return _threadDbContext.Threads
            .Include(t => t.ThreadComments)
            .Include(t => t.User) // Include the User navigation property
            .ToList();
    }

    public Thread GetThreadById(int threadId)
    {
        var thread = _threadDbContext.Threads
            .Include(t => t.ThreadComments)
            .ThenInclude(c => c.User)
            .FirstOrDefault(t => t.ThreadId == threadId);
            _threadDbContext.Entry(thread)
            .Reference(t => t!.User)
            .Load();
            return thread;
    }

    public async Task<bool> Add(Thread thread)
    {
        try
        {
            _threadDbContext.Threads.Add(thread);
            await _threadDbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[ItemRepository] item creation failed for thread {@thread}, error message: {e}", thread, e.Message);
            return false;
        }
    }

    public async Task Update(Thread thread)
    {
        _threadDbContext.Threads.Update(thread);
    }

    public async Task Remove(Thread thread)
    {
        _threadDbContext.Threads.Remove(thread);
    }

    public async Task SaveChangesAsync()
    {
        await _threadDbContext.SaveChangesAsync();
    }
    public IEnumerable<Thread> SearchPosts(string searchQuery)
    {
        var searchResults = _threadDbContext.Threads
            .Where(thread => EF.Functions.Like(thread.ThreadTitle, $"%{searchQuery}%") ||
                             EF.Functions.Like(thread.ThreadBody, $"%{searchQuery}%"))
            .ToList();

        return searchResults;
    }


}
