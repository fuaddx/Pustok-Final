using System.ComponentModel.DataAnnotations;

namespace Pustok2.ViewModel.AuthVM
{
    public class LoginVm
    {
		public string UsernameOrEmail { get; set; }
		[DataType(DataType.Password)]
		public string Password { get; set; }

		public bool IsRemember { get; set; }
	}
}
