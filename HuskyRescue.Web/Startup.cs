using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using Owin;

[assembly: OwinStartupAttribute(typeof(HuskyRescue.Web.Startup))]
namespace HuskyRescue.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
			HuskyRescue.BusinessLogic.Identity.Startup.DataProtectionProvider = app.GetDataProtectionProvider();
            ConfigureAuth(app);
        }
    }
}
