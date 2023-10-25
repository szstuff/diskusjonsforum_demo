using System;
using Diskusjonsforum.Models;

namespace diskusjonsforum.ViewModels
{
    public class UserListViewModel
    {
        public IEnumerable<ApplicationUser> Users;
        public string? CurrentViewName;

        public UserListViewModel(IEnumerable<ApplicationUser> users, string? currentViewName)
        {
            Users = users;
            CurrentViewName = currentViewName;

        }
    }
}