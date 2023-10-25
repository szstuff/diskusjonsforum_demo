using System.Net;
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

	public async Task<IActionResult> MakeAdmin()
	{
		if (HttpContext.User.Identity!.IsAuthenticated)
		{
			var user = await _userManager.GetUserAsync(HttpContext.User);
        
			if (user != null)
			{
				var userRoles = await _userManager.GetRolesAsync(user);

				if (userRoles.Contains("Admin"))
				{
					await _userManager.RemoveFromRoleAsync(user, "Admin");
				}
				else
				{
					await _userManager.AddToRoleAsync(user, "Admin");
				}
			}
		}

		return RedirectToAction("Index", "Home");
	}
}