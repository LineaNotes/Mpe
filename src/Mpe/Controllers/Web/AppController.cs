using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Mpe.Models;

namespace Mpe.Controllers.Web
{

	public class AppController : Controller
	{
		private readonly IGasRepository _repository;

		public AppController(IGasRepository repository)
		{
			_repository = repository;
		}

		public IActionResult Index()
		{
			return View();
		}

		[Authorize]
		public IActionResult Price()
		{
			return View();
		}

		[Authorize]
		public IActionResult Lookup()
		{
			return View();
		}

		[Authorize]
		public IActionResult Graph()
		{
			return View();
		}
	}
}