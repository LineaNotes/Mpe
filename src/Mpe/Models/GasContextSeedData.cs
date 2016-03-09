using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace Mpe.Models
{
	public class GasContextSeedData
	{
		private GasContext _context;
		private UserManager<GasUser> _userManager;

		public GasContextSeedData(GasContext context, UserManager<GasUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public async Task EnsureSeedDataAsync()
		{
			if (await _userManager.FindByEmailAsync("test1@test.si") == null)
			{
				var newUser = new GasUser()
				{
					UserName = "nivoAdmin",
					Email = "test1@test.si"
				};

				await _userManager.CreateAsync(newUser, "!P@ssw0rd");

				var newUser1 = new GasUser()
				{
					UserName = "nivoVodstveni",
					Email = "test2@test.si"
				};

				await _userManager.CreateAsync(newUser1, "!P@ssw0rd");

				var newUser2 = new GasUser()
				{
					UserName = "nivoTajnik",
					Email = "test3@test.si"
				};

				await _userManager.CreateAsync(newUser2, "!P@ssw0rd");
			}

			var rec = _context.Gases.FirstOrDefault();

			if (rec == null)
			{
				string jsonString = File.ReadAllText(@"files\gas.json");

				Gas[] gasArray = JsonConvert.DeserializeObject<Gas[]>(jsonString);
				_context.Gases.AddRange(gasArray);

				await _context.SaveChangesAsync();
			}

			var rec1 = _context.GasPrices.FirstOrDefault();

			if (rec1 == null)
			{
				string jsonString = File.ReadAllText(@"files\gasPrice.json");

				GasPrice[] gasPriceArray = JsonConvert.DeserializeObject<GasPrice[]>(jsonString);
				_context.GasPrices.AddRange(gasPriceArray);

				_context.SaveChanges();
			}
		}
	}
}