using Microsoft.AspNetCore.Identity;

namespace Huawei.Btk.Core.Models
{
	public class User : IdentityUser<Guid>
	{
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public int Tall { get; set; }
        public int Weight { get; set; }
        public string DefaultLanguage { get; set; }
        public List<Allergy> Allergies { get; set; }
        public List<ActiveIngredient> ActiveIngredients { get; set; }
        public List<Analysis> Analyses { get; set; }
	}
}
