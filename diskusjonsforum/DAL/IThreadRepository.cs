using Thread = Diskusjonsforum.Models.Thread;

namespace diskusjonsforum.DAL;

public interface IThreadRepository
{
    IEnumerable<Thread> GetAll();
    Thread GetThreadById(int threadId);
    void Add(Thread thread);
    Task Update(Thread thread);
    Task Remove(Thread thread);
    Task SaveChangesAsync();
}
