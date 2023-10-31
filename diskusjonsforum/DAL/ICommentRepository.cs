using Diskusjonsforum.Models;
using Thread = Diskusjonsforum.Models.Thread;

namespace diskusjonsforum.DAL;


public interface ICommentRepository
{
    IEnumerable<Comment> GetAll();
    Comment GetById(int? commentId);
    IQueryable<Comment> GetThreadComments(Thread thread);
    Task<bool> Add(Comment comment);
    Task<bool> Update(Comment comment);
    void Remove(Comment comment);
    Task SaveChangesAsync();
    List<Comment> GetChildren(Comment parentComment);

    void DetachEntity(Comment comment);
}