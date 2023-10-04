using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Diskusjonsforum.Models

{
	public class User
	{
		//[Key]
		//[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int UserId { get; set; } 
        public String Name { get; set; } = null!; //null! "promises" the compiler that the value wont be null. Use Regex!
        public String Email { get; set; } = null!; //null! "promises" the compiler that the value wont be null. Use Regex!
        public String PasswordHash { get; set; } = null!; //null! "promises" the compiler that the value wont be null. Use Regex!
        public bool Administrator { get; set; } = false;


    }
	

}
