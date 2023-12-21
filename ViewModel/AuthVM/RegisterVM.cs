using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Pustok2.ViewModel.AuthVM 
{
    public class RegisterVM
    {
		/*[Required(ErrorMessage = "Ad və soy adınızı daxil edin!!!"), MaxLength(36)]
		public string Fullname { get; set; }*/
		[Required(ErrorMessage = "Ad daxil edin!!!"), MaxLength(36)]
		public string Fisrtname { get; set; }
		[Required(ErrorMessage = "Soyad daxil edin!!!"), MaxLength(36)]
		public string Lastname { get; set; }
		[Required, DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		[Required(ErrorMessage = "İstifadəçi adınızı daxil edin!!!"), MaxLength(24)]
		public string Username { get; set; }
		[Required, DataType(DataType.Password), Compare(nameof(ConfirmPassword)), RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{4,}$", ErrorMessage = "Wrong input for password")]
		public string Password { get; set; }
		[Required, DataType(DataType.Password), RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{4,}$", ErrorMessage = "Wrong input for password")]
		public string ConfirmPassword { get; set; }
	}
}
