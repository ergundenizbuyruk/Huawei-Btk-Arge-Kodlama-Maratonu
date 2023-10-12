using Huawei.Btk.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Huawei.Btk.Core.Context
{
	public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>
	{
        public DbSet<ActiveIngredient> ActiveIngredients { get; set; }
        public DbSet<Allergy> Allergies { get; set; }
        public DbSet<Analysis> Analyses { get; set; }
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}
	}
}
