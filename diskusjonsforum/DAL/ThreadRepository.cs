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

    public IEnumerable<Thread> GetAll(string cookie)
    {
        return _threadDbContext.Threads.Where(t => t.UserCookie == cookie || t.UserCookie == "stilian").Include(t => t.ThreadComments)
            .ToList();
    }

    public Thread GetThreadById(int threadId, string cookie)
    {
        var thread = _threadDbContext.Threads
            .Include(t => t.ThreadComments.Where(t => t.UserCookie == cookie || t.UserCookie == "stilian"))
            .FirstOrDefault(t => t.ThreadId == threadId);
            //Access the entry for the thread to load Users into memory. Prevents issue where Users sometimes aren't loaded despite being included
            _threadDbContext.Entry(thread);
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

    public async Task<bool> Update(Thread thread)
    {
        _threadDbContext.Threads.Update(thread);
        await _threadDbContext.SaveChangesAsync();
        return true;
    }

    public async Task Remove(Thread thread)
    {
        _threadDbContext.Threads.Remove(thread);
    }

    public async Task SaveChangesAsync()
    {
        await _threadDbContext.SaveChangesAsync();
    }

}
