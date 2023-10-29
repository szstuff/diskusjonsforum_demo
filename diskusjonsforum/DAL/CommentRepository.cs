using Diskusjonsforum.Models;
using Microsoft.EntityFrameworkCore;
using Thread = Diskusjonsforum.Models.Thread;

namespace diskusjonsforum.DAL;

public class CommentRepository : ICommentRepository
{
    private readonly ThreadDbContext _threadDbContext; 

    public CommentRepository(ThreadDbContext threadDbContext)
    {
        _threadDbContext = threadDbContext;
    }

    public IEnumerable<Comment> GetAll()
    {
        return _threadDbContext.Comments.Include(c => c.User).ToList();
    }

    public Comment GetById(int? commentId)
    {
        return _threadDbContext.Comments
            .Include(c => c.User)
            .FirstOrDefault(c => c.CommentId == commentId);
    }

    public IQueryable<Comment> GetThreadComments(Thread thread)
    {
        return _threadDbContext.Comments.Where(comment => comment.ThreadId == thread.ThreadId).Include(t => t.User).Include(t=>t.ParentComment);
    }


    public void Add(Comment comment)
    {
        _threadDbContext.Comments.Add(comment);
    }

    public void Update(Comment comment)
    {
        _threadDbContext.Comments.Update(comment);
    }

    public void Remove(Comment comment)
    {
        _threadDbContext.Comments.Remove(comment);
    }

    public async Task SaveChangesAsync()
    {
        await _threadDbContext.SaveChangesAsync();
    }

    public List<Comment> GetChildren(Comment parentComment)
    {
        return _threadDbContext.Comments.Where(c => c.ParentCommentId == parentComment.CommentId).ToList();
    }
}
