using System;
using Diskusjonsforum.Models;
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
		List<Diskusjonsforum.Models.ApplicationUser> users = _threadDbContext.Users.ToList();
		var userListViewModel = new UserListViewModel(users, "Table");
		return View(userListViewModel);
	}
	
	public List<ApplicationUser> GetUsers()
	{
		var users = new List<ApplicationUser>();
		return users;
	}
}
