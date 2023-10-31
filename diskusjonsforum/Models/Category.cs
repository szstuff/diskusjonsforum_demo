
/*class for category*/
using System.Collections;

namespace Diskusjonsforum.Models;

public class Category
{
    public string CategoryName { get; set; }
    public string? CategoryDescription { get; set; } // Some categories have descriptions, but we decided that the name is self-explanatory. Description is nullable as it's not used or required 
    public bool RestrictedToAdmins { get; set; } = false;  //Same as above: Unused property of Category. Defaults to false 
    public List<Thread>? ThreadsInCategory { get; set; }
}
