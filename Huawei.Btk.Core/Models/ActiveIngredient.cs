using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huawei.Btk.Core.Models
{
	public class ActiveIngredient
	{
		public int Id { get; set; }
		public string ActiveIngredientText { get; set; }
		public User User { get; set; }
		public Guid UserId { get; set; }
	}
}
