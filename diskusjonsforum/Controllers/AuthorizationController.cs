using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Diskusjonsforum.Models;
using Microsoft.Extensions.Logging;

namespace diskusjonsforum.Controllers;

public class AuthorizationController : Controller
{
    private readonly ILogger<AuthorizationController> _logger;

    public AuthorizationController(ILogger<AuthorizationController> logger)
    {
        _logger = logger;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        _logger.LogError("An error occurred. RequestId: {RequestId}", HttpContext.TraceIdentifier);

        var errormsg = "[CommentController] An error occurred in the Create action";
        _logger.LogError("[CommentController] An error occurred. RequestId: {RequestId}");
        return RedirectToAction("Error", "Home", new { errormsg });
    }

    public IActionResult Register()
    {
        try
        { 
            // Log a successful registration
            _logger.LogInformation("User registered successfully.");

            return View();
        }
        catch (Exception ex)
        {
            // Log the exception
            _logger.LogError(ex, "An error occurred during registration.");

            // Handle the error and return an error view or message
            return View("Error"); // You can create an Error.cshtml view
        }
    }
}