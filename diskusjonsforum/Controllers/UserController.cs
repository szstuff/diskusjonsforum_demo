using System;
using diskusjonsforum.Models;
using diskusjonsforum.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Thread = System.Threading.Thread;

public class UserController : Controller
{
	private readonly ThreadDbContext _threadDbContext;

	public UserController(ThreadDbContext threadDbContext)
	{
		_threadDbContext = threadDbContext;
	}

	public IActionResult Table()
	{
		List<User> users = _threadDbContext.Users.ToList();
		var userListViewModel = new UserListViewModel(users, "Table");
		return View(userListViewModel);
	}
	
	public List<User> GetUsers()
	{
		var users = new List<User>();
		var user1 = new User
		{
			Name = "Stilian",
			Email="stilian@test.com",
			Administrator = true,
			PasswordHash = "password"
		};
		users.Add(user1);
		return users;
	}
}
