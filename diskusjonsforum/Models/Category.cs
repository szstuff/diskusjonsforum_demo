
/*class for category*/
using System.Collections;

namespace Diskusjonsforum.Models;

public class Category : IEnumerable
{
    public string CategoryName { get; set; }
    public string? CategoryDescription { get; set; } /*the questionmark indicates that it can be nullable*/
    public bool RestrictedToAdmins { get; set; } = false;  /*set to false meaning the category is not restricted to admins by default*/
    public List<Thread>? ThreadsInCategory { get; set; }


    public IEnumerator GetEnumerator()
    {
        throw new NotImplementedException();
    }
}