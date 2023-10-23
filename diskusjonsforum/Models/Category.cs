namespace Diskusjonsforum.Models;

public class Category
{
    public string CategoryName { get; set; }
    public string? CategoryDescription { get; set; }
    public bool RestrictedToAdmins { get; set; } = false; 
    
}