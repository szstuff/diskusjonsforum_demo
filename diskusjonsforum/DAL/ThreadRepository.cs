using Diskusjonsforum.Models;
using Microsoft.EntityFrameworkCore;
using Thread = Diskusjonsforum.Models.Thread;

namespace diskusjonsforum.DAL;

public class ThreadRepository : IThreadRepository
{
    private readonly ThreadDbContext _threadDbContext;

    public ThreadRepository(ThreadDbContext threadDbContext)
    {
        _threadDbContext = threadDbContext;
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

    public void Add(Thread thread)
    {
        _threadDbContext.Threads.Add(thread);
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
}
