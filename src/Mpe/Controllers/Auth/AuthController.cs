using System.Threading.Tasks;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using Mpe.Models;
using Mpe.ViewModels;

namespace Mpe.Controllers.Auth
{
	public class AuthController : Controller
	{
		private SignInManager<GasUser> _signInManager;

		public AuthController(SignInManager<GasUser> signInManager)
		{
			_signInManager = signInManager;
		}

		[Authorize]
		public IActionResult Admin()
		{
			return View();
		}

		public IActionResult Login()
		{
			if (User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Lookup", "App");
			}

			return View();
		}

		[HttpPost]
		public async Task<ActionResult> Login(LoginViewModel vm, string returnUrl)
		{
			if (ModelState.IsValid)
			{
				var signInResult = await _signInManager.PasswordSignInAsync(vm.Username,
					vm.Password,
					true, false);

				if (signInResult.Succeeded)
				{
					if (string.IsNullOrWhiteSpace(returnUrl))
					{
						return RedirectToAction("Lookup", "App");
					}
					else
					{
						return Redirect(returnUrl);
					}
				}
				else
				{
					ModelState.AddModelError("", "Username or password incorrect");
				}
			}

			return View();
		}

		public async Task<ActionResult> Logout()
		{
			if (User.Identity.IsAuthenticated)
			{
				await _signInManager.SignOutAsync();
			}

			return RedirectToAction("Index", "App");
		}

		[Authorize]
		public IActionResult Register()
		{
			return View();
		}
	}
}