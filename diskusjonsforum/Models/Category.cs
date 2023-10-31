
/*class for category*/
using System.Collections;

namespace Diskusjonsforum.Models;

public class Category
{
    public string CategoryName { get; set; } //Doubles as PK 

    public List<Thread>? ThreadsInCategory { get; set; }
}
