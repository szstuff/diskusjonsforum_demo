using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using diskusjonsforum.Models;
using diskusjonsforum.ViewModels;
using Thread = diskusjonsforum.Models.Comment;

//using diskusjonsforum.ViewModels; //Kan slettes hvis vi ikke lager ViewModels

namespace diskusjonsforum.Controllers;


public class CommentController : Controller
{
    private readonly ThreadDbContext _threadDbContext;

    public CommentController(ThreadDbContext threadDbContext)
    {
        _threadDbContext = threadDbContext;
    }

    public IActionResult Table()
    {
        List<Comment> comments = _threadDbContext.Comments.ToList();
        var commentListViewModel = new CommentListViewModel(comments, "Table");
        return View(commentListViewModel);
    }

    public List<Comment> GetComments()
    {
        var comments = new List<Comment>();
        return comments;
    }
}
