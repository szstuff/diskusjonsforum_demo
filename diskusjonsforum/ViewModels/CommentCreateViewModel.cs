using Diskusjonsforum.Models;
using System.ComponentModel.DataAnnotations;
using Thread = Diskusjonsforum.Models.Thread;

namespace diskusjonsforum.ViewModels;

public class CommentCreateViewModel
{
    public int ThreadId { get; set; }
    public int? ParentCommentId { get; set; }
    public Comment? ParentComment { get; set; }
    public Comment? CommentToEdit { get; set; }
    public Comment? Comment { get; set; }
    public Thread? Thread { get; set; }
    [Required(ErrorMessage = "Please enter a comment.")]
    public string CommentBody { get; set; }
}