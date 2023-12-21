using System.ComponentModel.DataAnnotations;

namespace Pustok2.ViewModel.UserPageVm
{
	public class UserPageVM
	{
		public string Username { get; set; }
		[MaxLength(36)]
		public string Name { get; set; }
		[MaxLength(36)]
		public string Surname { get; set; }
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		[Required, DataType(DataType.Password), Compare(nameof(ConfirmPassword)), RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{4,}$", ErrorMessage = "Wrong input for password")]
		public string Password { get; set; }
		[Required, DataType(DataType.Password), RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{4,}$", ErrorMessage = "Wrong input for password")]
		public string ConfirmPassword { get; set; }
		public string ProfileImageURL { get; set; }
		public IFormFile? ProfileImage { get; set; }
	}
}
