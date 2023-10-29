using Diskusjonsforum.Models;
using diskusjonsforum.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace diskusjonsforum.Controllers;

public class UserController : Controller
{
	private UserManager<ApplicationUser> _userManager;
	private readonly ILogger<UserController> _logger;

	public UserController(UserManager<ApplicationUser> userManager, ILogger<UserController> logger)
	{
		_userManager = userManager;
		_logger = logger;
	}

	public IActionResult Table()
    {
        try
        {
            var users = _userManager.Users.ToList();
            var userListViewModel = new UserListViewModel(users, "Table");
            return View(userListViewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[UserController] An error occurred in the Table method.");
            var errormsg = "[UserController] An error occured in the Table method.";
            return RedirectToAction("Error", "Home", new {errormsg});
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
        var errorMsg = "An error occured when attempting to switch your roles";
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
                else
                {
                    errorMsg = "An error occured when authenticating you"; 
                    _logger.LogError("[UserController] An error occurred in the MakeAdmin method.");
                    return RedirectToAction("Error", "Home", new {errorMsg});
                }
            } else
            {
                errorMsg = "An error occured when authenticating you"; 
                _logger.LogError("[UserController] An error occurred in the MakeAdmin method.");
                return RedirectToAction("Error", "Home", new {errorMsg});
            }

            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            errorMsg = "An error occured when attempting to switch your roles. Verify that the requested role exists."; 
            _logger.LogError(ex, "[UserController] An error occurred in the MakeAdmin method.");
            return RedirectToAction("Error", "Home", new {errorMsg});
        }
    }
}