using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Diskusjonsforum.Models;

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
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult Register()
    {
        return View();
    }
}