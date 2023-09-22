using System;
using diskusjonsforum.Models;
using Thread = diskusjonsforum.Models.Comment;

namespace diskusjonsforum.ViewModels
{
    public class CommentListViewModel
    {
        public IEnumerable<Comment> Comments;
        public string? CurrentViewName;

        public CommentListViewModel(IEnumerable<Comment> comments, string? currentViewName)
        {
            Comments = comments;
            CurrentViewName = currentViewName;

        }
    }
}