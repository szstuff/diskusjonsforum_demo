using System;
using Diskusjonsforum.Models;
using Thread = Diskusjonsforum.Models.Comment;

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