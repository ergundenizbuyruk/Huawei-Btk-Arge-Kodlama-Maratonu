using System.ComponentModel.DataAnnotations;

namespace Huawei.Btk.Application.Services.UserServices.Dtos
{
	public class LoginDto
	{
		[Display(Name = "E-mail")]
		[Required(ErrorMessage = "Lütfen e-posta adresinizi giriniz.")]
		[EmailAddress]
		public string Email { get; set; }

		[Display(Name = "Password")]
		[Required(ErrorMessage = "Lütfen parolanızı giriniz.")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}
