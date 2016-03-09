using Microsoft.Extensions.Logging;

namespace Mpe.Models
{
	public class GasRepository : IGasRepository
	{
		private GasContext _context;
		private ILogger<GasRepository> _logger;

		public GasRepository(GasContext context, ILogger<GasRepository> logger)
		{
			_context = context;
			_logger = logger;
		}

		public bool SaveAll()
		{
			return _context.SaveChanges() > 0;
		}
	}
}