using System.Net;
using System.Security.Claims;
using Diskusjonsforum.Models;
using diskusjonsforum.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace diskusjonsforum.Controllers;

public class UserController : Controller
{
	private readonly ThreadDbContext _threadDbContext;
	private UserManager<ApplicationUser> _userManager;
	private readonly ILogger<UserController> _logger;

	public UserController(ThreadDbContext threadDbContext, UserManager<ApplicationUser> userManager, ILogger<UserController> logger)
	{
		_threadDbContext = threadDbContext;
		_userManager = userManager;
		_logger = logger;
	}

	public IActionResult Table()
    {
        try
        {
            var users = _threadDbContext.Users.ToList();
            var userListViewModel = new UserListViewModel(users, "Table");
            return View(userListViewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[UserController] An error occurred in the Table method.");
            return View("Error");
        }
    }

    public List<ApplicationUser> GetUsers()
    {
        try
        {
            var users = new List<ApplicationUser>();
            return users;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[UserController] An error occurred in the GetUsers method.");
            return new List<ApplicationUser>();
        }
    }

    public async Task<IActionResult> MakeAdmin()
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "[UserController] An error occurred in the MakeAdmin method.");
            return RedirectToAction("Error", "Home");
        }
    }
}