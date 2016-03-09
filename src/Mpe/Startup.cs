using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json.Serialization;
using Mpe.Models;

namespace Mpe
{
	public class Startup
	{
		public static IConfigurationRoot Configuration;

		public Startup(IApplicationEnvironment appEnv)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(appEnv.ApplicationBasePath)
				.AddJsonFile("config.json")
				.AddEnvironmentVariables();

			Configuration = builder.Build();
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc(config =>
			{
#if !DEBUG
				config.Filters.Add(new RequireHttpsAttribute());
#endif
			}).AddJsonOptions(opt => { opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); });

			services.AddIdentity<GasUser, IdentityRole>(config =>
			{
				config.User.RequireUniqueEmail = true;
				config.Password.RequiredLength = 8;
				config.Cookies.ApplicationCookie.LoginPath = "/Auth/Login";
				config.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents()
				{
					OnRedirectToLogin = ctx =>
					{
						if (ctx.Request.Path.StartsWithSegments("/api")
						    && ctx.Response.StatusCode == (int) HttpStatusCode.OK)
						{
							ctx.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
						}
						else
						{
							ctx.Response.Redirect(ctx.RedirectUri);
						}
						return Task.FromResult(0);
					}
				};
			})
				.AddEntityFrameworkStores<GasContext>();

			services.AddLogging();

			services.AddEntityFramework()
				.AddSqlServer()
				.AddDbContext<GasContext>();

			services.AddTransient<GasContextSeedData>();
			services.AddScoped<IGasRepository, GasRepository>();
		}

		public async void Configure(IApplicationBuilder app, GasContextSeedData seeder, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddDebug(LogLevel.Warning);

			app.UseStaticFiles();

			app.UseIdentity();

			Mapper.Initialize(config => { }
				);

			app.UseMvc(config =>
			{
				config.MapRoute(
					name: "Default",
					template: "{controller}/{action}/{id?}",
					defaults:
						new {controller = "App", action = "Index"}
					);
			});

			await seeder.EnsureSeedDataAsync();
		}

		public static void Main(string[] args) => WebApplication.Run<Startup>(args);
	}
}