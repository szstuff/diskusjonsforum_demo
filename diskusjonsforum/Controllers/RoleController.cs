using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace diskusjonsforum.Controllers;

public class RoleController : Controller
{
    private RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<RoleController> _logger;

    public RoleController(RoleManager<IdentityRole> roleManager, ILogger<RoleController> logger)
    {
        _roleManager = roleManager;
        _logger = logger;
    }

    private void Errors(IdentityResult result)
    {
        foreach (IdentityError error in result.Errors)
            ModelState.AddModelError("", error.Description);
    }
 
    public IActionResult Create() => View();
 
    [HttpPost]
    public async Task<IActionResult> Create([Required] string name)
    {
        if (ModelState.IsValid)
        {
            IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Errors(result);
                _logger.LogError("[RoleController] Error creating role: {0}", name);
            }    
        }
        return View(name);
    }
}