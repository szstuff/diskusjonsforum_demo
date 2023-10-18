using Microsoft.AspNetCore.Identity;

namespace Diskusjonsforum.Models;

public class XApplicationUser : ApplicationUser
{
    public string Username { get; set; }
}