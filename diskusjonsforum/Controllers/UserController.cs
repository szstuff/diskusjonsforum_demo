using diskusjonsforum.DAL;
using Diskusjonsforum.Models;
using diskusjonsforum.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Thread = Diskusjonsforum.Models.Thread;

namespace diskusjonsforum.Controllers;

public class UserController : Controller
{
    //Initialise controllers and interfaces for constructor
	private UserManager<ApplicationUser> _userManager;
    private readonly IThreadRepository _threadRepository;
    private readonly ICommentRepository _commentRepository;
	private readonly ILogger<UserController> _logger;
	public UserController(UserManager<ApplicationUser> userManager, IThreadRepository threadRepository, ICommentRepository commentRepository, ILogger<UserController> logger)
	{
		_userManager = userManager;
		_logger = logger;
        _threadRepository = threadRepository;
        _commentRepository = commentRepository;
    }

	public async Task<IActionResult> Table()
    {
        var errorMsg = "";
        var user = await _userManager.GetUserAsync(HttpContext.User);
        var userRoles = await _userManager.GetRolesAsync(user);
        if (!userRoles.Contains("Admin"))
        {
            errorMsg = "You are not authorised to access this page";
            return RedirectToAction("Error", "Home", new { errorMsg });
        }

        try
        {
            var users = _userManager.Users.ToList();
            var userListViewModel = new UserListViewModel(users, "Table");
            return View(userListViewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[UserController] An error occurred in the Table method.");
            errorMsg = "[UserController] An error occured in the Table method.";
            return RedirectToAction("Error", "Home", new {errorMsg});
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