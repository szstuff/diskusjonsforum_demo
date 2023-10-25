using System.Collections;

namespace Diskusjonsforum.Models;

public class Category : IEnumerable
{
    public string CategoryName { get; set; }
    public string? CategoryDescription { get; set; }
    public bool RestrictedToAdmins { get; set; } = false; 
    public List<Thread>? ThreadsInCategory { get; set; }


    public IEnumerator GetEnumerator()
    {
        throw new NotImplementedException();
    }
}