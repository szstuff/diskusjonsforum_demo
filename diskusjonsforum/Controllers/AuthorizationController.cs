using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Diskusjonsforum.Models;
using Microsoft.Extensions.Logging;

namespace diskusjonsforum.Controllers;

public class AuthenticationController : Controller
{
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(ILogger<AuthenticationController> logger)
    {
        _logger = logger;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        _logger.LogError("An error occurred. RequestId: {RequestId}", HttpContext.TraceIdentifier);

        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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