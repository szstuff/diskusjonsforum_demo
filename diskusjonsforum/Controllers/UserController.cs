using System.Security.Claims;
using Diskusjonsforum.Models;
using diskusjonsforum.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace diskusjonsforum.Controllers;

public class UserController : Controller
{
	private readonly ThreadDbContext _threadDbContext;
	private UserManager<ApplicationUser> _userManager;
	public UserController(ThreadDbContext threadDbContext, UserManager<ApplicationUser> userManager)
	{
		_threadDbContext = threadDbContext;
		_userManager = userManager;
	}

	public IActionResult Table()
	{
		var users = _threadDbContext.Users.ToList();
		var userListViewModel = new UserListViewModel(users, "Table");
		return View(userListViewModel);
	}
	
	public List<ApplicationUser> GetUsers()
	{
		var users = new List<ApplicationUser>();
		return users;
	}
	
	public async void MakeAdmin()
	{
		var user = await _userManager.GetUserAsync(HttpContext.User);
		await _userManager.AddToRoleAsync(user, "Admin");
		
	}
}