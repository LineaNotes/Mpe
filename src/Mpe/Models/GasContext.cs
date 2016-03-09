using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;

namespace Mpe.Models
{
	public class GasContext : IdentityDbContext<GasUser>
	{
		public GasContext()
		{
			Database.EnsureCreated();
		}

		public DbSet<Gas> Gases { get; set; }
		public DbSet<GasPrice> GasPrices { get; set; }
		public DbSet<Log> Logs { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			var connString = Startup.Configuration["Data:MpeContextConnection"];

			optionsBuilder.UseSqlServer(connString);

			base.OnConfiguring(optionsBuilder);
		}
	}
}