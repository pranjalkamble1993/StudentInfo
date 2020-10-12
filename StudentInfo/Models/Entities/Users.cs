namespace StudentInfo.Models.Entities
{
	public class Users
	{
		public int Id { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public UserType UserType { get; set; }

		public string Email { get; set; }

		public string Phone { get; set; }

		public bool Active { get; set; }
	}

	public enum UserType
	{
		Student = 4,

		Parent = 5
	}
}
