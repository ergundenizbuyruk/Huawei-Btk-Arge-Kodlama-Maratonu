using System.ComponentModel.DataAnnotations;

namespace Huawei.Btk.Application.Services.UserServices.Dtos
{
	public class UserRegisterDto
	{
		[Display(Name = "Ad")]
		[Required(ErrorMessage = "Lütfen adınızı giriniz.")]
		public string Name { get; set; }

		[Display(Name = "Soyad")]
		[Required(ErrorMessage = "Lütfen soyadınızı giriniz.")]
		public string Surname { get; set; }

		[Display(Name = "E-posta")]
		[Required(ErrorMessage = "Lütfen e-posta adresinizi giriniz.")]
		[EmailAddress]
		public string Email { get; set; }

		[Required(ErrorMessage = "Lütfen telefon numaranızı giriniz.")]
		[Phone]
		[Display(Name = "Telefon Numarası")]
		public string PhoneNumber { get; set; }

		[Display(Name = "Yaş")]
		public int Age { get; set; }

		[Display(Name = "Uzunluk (cm)")]
		public int Tall { get; set; }

		[Display(Name = "Ağırlık (kg)")]
		public int Weight { get; set; }

		[Display(Name = "Varsayılan dil")]
		public string DefaultLanguage { get; set; }

		[Display(Name = "Parola")]
		[Required(ErrorMessage = "Lütfen parolanızı giriniz.")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Display(Name = "Parola Tekrar")]
		[Required(ErrorMessage = "Lütfen parolanızı giriniz.")]
		[DataType(DataType.Password)]
		[Compare(nameof(Password), ErrorMessage = "Parolalar eşleşmiyor")]
		public string PasswordConfirm { get; set; }
	}
}
