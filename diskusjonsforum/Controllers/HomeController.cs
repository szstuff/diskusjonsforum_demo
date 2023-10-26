using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Diskusjonsforum.Models;

namespace diskusjonsforum.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Error(string errorMsg)
    {
        //Log error message using logger
        _logger.LogError("[HomeController] Error: {0}", errorMsg);
        //View error message in view
        ViewBag.ErrorMsg = errorMsg;
        return View();
    }
}

