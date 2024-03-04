using System.ComponentModel.DataAnnotations.Schema;

namespace Diskusjonsforum.Models
{
    public class User
    {
        public string Username { get; set; }
        public string CookieValue = null; 
    }
}