using System.ComponentModel.DataAnnotations;

namespace StudentInfo.Models
{
    /// <summary>
    /// Student data
    /// </summary>
    public class StudentData
	{
		public int Id { get; set; }

		[Required]
		[Display(Name = "StudentName")]
		[DataType(DataType.Text)]
		[RegularExpression(@"^[a-zA-Z]+(\s[a-zA-Z]+)?$", ErrorMessage = "Please enter First name and last name")]
		public string StudentName { get; set; }

		[Display(Name = "ParentName")]
		public string ParentName { get; set; }

		[Required(ErrorMessage = "Required")]
		[Display(Name = "StudentClass")]
		public string StudentClass { get; set; }

		[Required(ErrorMessage = "Required")]
		[EmailAddress(ErrorMessage = "Please enter valid parent email address")]
		[Display(Name = "ParentEmail")]
		public string ParentEmail { get; set; }

		[Required(ErrorMessage = "Required")]		
		[Display(Name = "ParentMobile")]
		[DataType(DataType.PhoneNumber)]
		[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
		public string ParentMobile { get; set; }

		public bool Active { get; set; }
	}
}
