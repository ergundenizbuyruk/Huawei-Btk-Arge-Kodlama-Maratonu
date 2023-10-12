using System.ComponentModel.DataAnnotations;

namespace Huawei.Btk.Application.Services.UserServices.Dtos
{
	public class UserDto
	{
		public string Name { get; set; }
		public string Surname { get; set; }
		public string PhoneNumber { get; set; }
		public int Age { get; set; }
		public int Tall { get; set; }
		public int Weight { get; set; }
		public string DefaultLanguage { get; set; }
		public List<string> Allergies { get; set; }
		public List<string> ActiveIngredients { get; set; }
        public string PhotoPath { get; set; }
    }
}
