using System;
namespace diskusjonsforum.Models

{
	public class User
	{
		public int UserID { get; set; }
        public  String Name { get; set; }
        public String Email { get; set; }
        public String PasswordHash { get; set; }
        public bool Administrator { get; set; } = false;


    }
}

