using Thread = Diskusjonsforum.Models.Thread;

namespace diskusjonsforum.DAL;

public interface IThreadRepository
{
    IEnumerable<Thread> GetAll(string cookie);
    Thread GetThreadById(int threadId, string cookie);
    Task <bool> Add(Thread thread);
    Task<bool>Update(Thread thread);
    Task Remove(Thread thread);
    Task SaveChangesAsync();
}
