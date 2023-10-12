using System.ComponentModel.DataAnnotations;

namespace Huawei.Btk.Application.Services.UserServices.Dtos
{
	public class UpdateUserDto
	{
		[Display(Name = "Name")]
		[Required(ErrorMessage = "Lütfen adınızı giriniz.")]
		public string Name { get; set; }

		[Display(Name = "Surname")]
		[Required(ErrorMessage = "Lütfen soyadınızı giriniz.")]
		public string Surname { get; set; }

		[Required(ErrorMessage = "Lütfen telefon numaranızı giriniz.")]
		[Phone]
		[Display(Name = "Phone Number")]
		public string PhoneNumber { get; set; }

		[Display(Name = "Age", Prompt = "Age")]
		public int Age { get; set; }

		[Display(Name = "Length (cm)")]
		public int Tall { get; set; }

		[Display(Name = "Weight (kg)")]
		public int Weight { get; set; }

		[Display(Name = "Default Language")]
		public string DefaultLanguage { get; set; }

		[Display(Name = "Please enter your allergy conditions, if any.")]
		public string? Allergies { get; set; }

		[Display(Name = "Please enter the products you specifically do not want, if any.")]
		public string? ActiveIngredients { get; set; }
    }
}
