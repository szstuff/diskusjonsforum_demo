using Diskusjonsforum.Models;
using Microsoft.EntityFrameworkCore;
using ILogger = Castle.Core.Logging.ILogger;
using Thread = Diskusjonsforum.Models.Thread;

namespace diskusjonsforum.DAL;

public class CommentRepository : ICommentRepository
{
    private readonly ThreadDbContext _threadDbContext; 

    private readonly ILogger<CommentRepository> _logger;
    
    public CommentRepository(ThreadDbContext threadDbContext, ILogger<CommentRepository> logger)
    {
        _logger = logger;
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
            .FirstOrDefault(c => c.CommentId == commentId)!;
    }

    public IQueryable<Comment> GetThreadComments(Thread thread)
    {
        return _threadDbContext.Comments.Where(comment => comment.ThreadId == thread.ThreadId).Include(t => t.User).Include(t=>t.ParentComment);
    }


    public async Task<bool> Add(Comment comment)
    {
        try
        {
            _threadDbContext.Comments.Add(comment);
            await _threadDbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[CommentRepository] comment creation failed for comment {@comment}, error message: {e}", comment, e.Message);
            return false;
        }
    }

    public async Task<bool> Update(Comment comment)
    {
        try
        {
            _threadDbContext.Comments.Update(comment);
            await _threadDbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[CommentRepository] comment creation failed for comment {@comment}, error message: {e}", comment, e.Message);
            return false;
        }
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
