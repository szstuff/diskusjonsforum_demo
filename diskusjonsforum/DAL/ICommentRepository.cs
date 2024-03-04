using Diskusjonsforum.Models;
using Thread = Diskusjonsforum.Models.Thread;

namespace diskusjonsforum.DAL;


public interface ICommentRepository
{
    List<Comment> GetAll(string cookie);
    Comment GetById(int? commentId);
    IQueryable<Comment> GetThreadComments(Thread thread);
    Task<bool> Add(Comment comment);
    Task<bool> Update(Comment comment);
    void Remove(Comment comment);
    Task SaveChangesAsync();
    List<Comment> GetChildren(Comment parentComment, string cookie);

    void DetachEntity(Comment comment);
}