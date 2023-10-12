using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huawei.Btk.Core.Models
{
	public class Allergy
	{
        public int Id { get; set; }
        public string AllergyText { get; set; }
        public User User { get; set; }
        public Guid UserId { get; set; }
    }
}
