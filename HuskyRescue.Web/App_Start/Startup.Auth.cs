using HuskyRescue.BusinessLogic.Identity;
using HuskyRescue.Web.App_Start;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace HuskyRescue.Web
{
	public partial class Startup
	{
		// For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
		public void ConfigureAuth(IAppBuilder app)
		{
			var container = AutoFacConfig.Startup();
			app.UseAutofacMiddleware(container);

			var configure = new IdentityConfig();

			app = configure.ConfigureAuth(app);
			app.UseAutofacMvc();
		}
	}
}