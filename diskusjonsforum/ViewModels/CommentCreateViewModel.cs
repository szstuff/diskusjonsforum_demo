using Diskusjonsforum.Models;
using Thread = Diskusjonsforum.Models.Thread;

namespace diskusjonsforum.ViewModels;

public class CommentCreateViewModel
{
    public int ThreadId { get; set; }
    public int ParentCommentId { get; set; }
    public Comment? ParentComment { get; set; }
    public Comment? CommentToEdit { get; set; }
    public Comment? Comment { get; set; }
    public Thread Thread { get; set; }
}