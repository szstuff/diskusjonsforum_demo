using System;
using diskusjonsforum.Models;
using Thread = diskusjonsforum.Models.Thread;

namespace diskusjonsforum.ViewModels
{
    public class UserListViewModel
    {
        public IEnumerable<User> Users;
        public string? CurrentViewName;

        public UserListViewModel(IEnumerable<User> users, string? currentViewName)
        {
            Users = users;
            CurrentViewName = currentViewName;

        }
    }
}