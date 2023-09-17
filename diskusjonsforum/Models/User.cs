using System;
namespace diskusjonsforum.Models

{
	public class User
	{
		public int UserID { get; set; }
        public String Name { get; set; } = null!; //null! "promises" the compiler that the value wont be null. Use Regex!
        public String Email { get; set; } = null!; //null! "promises" the compiler that the value wont be null. Use Regex!
        public String PasswordHash { get; set; } = null!; //null! "promises" the compiler that the value wont be null. Use Regex!
        public bool Administrator { get; set; } = false;


    }
}

